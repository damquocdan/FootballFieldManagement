using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballFieldManagement.Models;

namespace FootballFieldManagement.Controllers
{
    public class BookingsController : Controller
    {
        private readonly FootballFieldManagementContext _context;

        public BookingsController(FootballFieldManagementContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var footballFieldManagementContext = _context.Bookings.Include(b => b.Customer).Include(b => b.Field);
            return View(await footballFieldManagementContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Field)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        // GET: Bookings/Create
        // GET: Bookings/Create
        public IActionResult Create(int fieldId)
        {
            // Retrieve the customer ID from session
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            // Check if the customer is logged in
            if (customerId == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để sử dụng chức năng này."; // Error message
                return RedirectToAction("Index", "LoginC"); // Redirect to login page
            }

            // Retrieve customer and field details
            var customer = _context.Customers.Find(customerId);
            var field = _context.Fields.Find(fieldId);

            if (customer == null || field == null)
            {
                return NotFound();
            }

            // Set the duration to the default value or use a passed value if applicable
            int duration = 2; // Default duration, can be adjusted
            var totalPrice = field.Price * duration;
            var totalServicePrice = 0; // Update if services are included

            // Set ViewData values to pass data to the view
            ViewData["CustomerName"] = customer.Username;
            ViewData["FieldName"] = field.FieldName;
            ViewData["FieldId"] = fieldId;
            ViewData["CustomerId"] = customerId;
            ViewData["TotalPrice"] = totalPrice;
            ViewData["TotalServicePrice"] = totalServicePrice;
            ViewData["TotalAmount"] = totalPrice + totalServicePrice;

            return View();
        }


        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FieldId,CustomerId,BookingTime,Duration")] Booking booking)
        {
            // Check if the field is available for the selected time and duration
            var isFieldAvailable = !_context.Bookings
                .Any(b => b.FieldId == booking.FieldId &&
                          b.BookingTime <= booking.BookingTime &&
                          b.BookingTime.AddHours(b.Duration ?? 0) > booking.BookingTime);

            if (!isFieldAvailable)
            {
                ModelState.AddModelError("", "Sân đã được đặt vào thời gian này.");

                // Reload field and customer details
                var field = _context.Fields.Find(booking.FieldId);
                var customer = _context.Customers.Find(booking.CustomerId);

                // Pass necessary data back to ViewData
                ViewData["FieldName"] = field?.FieldName ?? "Unknown Field";
                ViewData["CustomerName"] = customer?.Username ?? "Unknown Customer";
                ViewData["FieldId"] = booking.FieldId;
                ViewData["CustomerId"] = booking.CustomerId;

                return View(booking);
            }

            // Tính toán tổng chi phí đặt sân
            var fieldDetails = await _context.Fields.FindAsync(booking.FieldId);
            if (fieldDetails != null)
            {
                // Tính TotalPrice từ giá sân và thời gian
                booking.TotalPrice = fieldDetails.Price * booking.Duration;

                // Tính TotalServicePrice nếu có dịch vụ liên quan
                // Bạn có thể lấy thông tin dịch vụ từ một bảng khác như BookingService
                var totalServicePrice = _context.BookingServices
                    .Where(bs => bs.BookingId == booking.BookingId)  // Lọc các dịch vụ liên quan đến booking này
                    .Sum(bs => bs.TotalPrice);  // Cộng tất cả giá dịch vụ

                booking.TotalServicePrice = totalServicePrice;

                // Tính TotalAmount (tổng giá trị của booking)
                booking.TotalAmount = booking.TotalPrice + booking.TotalServicePrice;
            }
            else
            {
                ModelState.AddModelError("", "Không tìm thấy thông tin sân.");
                return View(booking);
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Reload field and customer details for invalid ModelState
            var fieldDetail = _context.Fields.Find(booking.FieldId);
            var customerDetail = _context.Customers.Find(booking.CustomerId);

            ViewData["FieldName"] = fieldDetail?.FieldName ?? "Unknown Field";
            ViewData["CustomerName"] = customerDetail?.Username ?? "Unknown Customer";
            ViewData["FieldId"] = booking.FieldId;
            ViewData["CustomerId"] = booking.CustomerId;

            return View(booking);
        }



        [HttpGet]
        public JsonResult CheckAvailability(int fieldId, DateTime bookingTime, int duration)
        {
            var existingBookings = _context.Bookings
                .Where(b => b.FieldId == fieldId)
                .ToList();

            var newBookingEndTime = bookingTime.AddHours(duration);

            var isOverlap = existingBookings.Any(b =>
            {
                var existingBookingEndTime = b.BookingTime.AddHours(b.Duration ?? 0)
;
                return (bookingTime < existingBookingEndTime && newBookingEndTime > b.BookingTime);
            });

            return Json(new { isOverlap });
        }


        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", booking.CustomerId);
            ViewData["FieldId"] = new SelectList(_context.Fields, "FieldId", "FieldId", booking.FieldId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,CustomerId,FieldId,BookingTime,Duration,TotalPrice,TotalServicePrice,TotalAmount")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Tính toán lại tổng giá trị sau khi chỉnh sửa
                    var fieldDetails = await _context.Fields.FindAsync(booking.FieldId);
                    if (fieldDetails != null)
                    {
                        // Cập nhật TotalPrice theo giá sân và thời gian
                        booking.TotalPrice = fieldDetails.Price * booking.Duration;

                        // Cập nhật TotalServicePrice nếu có thay đổi dịch vụ
                        var totalServicePrice = _context.BookingServices
                            .Where(bs => bs.BookingId == booking.BookingId)  // Lọc các dịch vụ liên quan đến booking này
                            .Sum(bs => bs.TotalPrice);  // Cộng tất cả giá dịch vụ

                        booking.TotalServicePrice = totalServicePrice;

                        // Cập nhật TotalAmount (tổng giá trị của booking)
                        booking.TotalAmount = booking.TotalPrice + booking.TotalServicePrice;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không tìm thấy thông tin sân.");
                        return View(booking);
                    }

                    // Cập nhật booking vào cơ sở dữ liệu
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Lấy thông tin khách hàng và sân để hiển thị lại trong View
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", booking.CustomerId);
            ViewData["FieldId"] = new SelectList(_context.Fields, "FieldId", "FieldId", booking.FieldId);
            return View(booking);
        }


        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Field)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
