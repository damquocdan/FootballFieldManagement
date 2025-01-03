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

            // Retrieve field details for the provided field ID
            // Retrieve customer and field details
            var customer = _context.Customers.Find(customerId);
            var field = _context.Fields.Find(fieldId);

            if (customer == null || field == null)
            {
                return NotFound();
            }
            int duration = 1;
            // Calculate values for TotalPrice and TotalServicePrice
            var totalPrice = field.Price * duration; // Assuming 'duration' is passed or predefined
            var totalServicePrice = 0; // Replace with actual service price calculation if any

            // Set ViewData values
            ViewData["CustomerName"] = customer.Username;
            ViewData["FieldName"] = field.FieldName;
            ViewData["FieldId"] = fieldId;
            ViewData["CustomerId"] = customerId;
            ViewData["TotalPrice"] = totalPrice;
            ViewData["TotalServicePrice"] = totalServicePrice;

            // Calculate TotalAmount as the sum of TotalPrice and TotalServicePrice
            ViewData["TotalAmount"] = totalPrice + totalServicePrice;


            return View();
        }


        // POST: Bookings/Create
        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FieldId,CustomerId,BookingTime,Duration")] Booking booking)
        {
            // Kiểm tra xem sân có sẵn vào thời gian đã chọn hay không
            var isFieldAvailable = !_context.Bookings
    .Any(b => b.FieldId == booking.FieldId && b.BookingTime <= booking.BookingTime &&
              b.BookingTime.AddHours(booking.Duration ?? 0) > booking.BookingTime);


            if (!isFieldAvailable)
            {
                ModelState.AddModelError("", "The selected field is not available at the chosen time.");

                // Lấy lại thông tin sân và khách hàng để hiển thị trên View nếu không có sẵn
                var field = _context.Fields.Find(booking.FieldId);
                var customer = _context.Customers.Find(booking.CustomerId);

                // Truyền các giá trị cần thiết vào ViewData
                ViewData["FieldName"] = field?.FieldName ?? "Unknown Field";
                ViewData["CustomerName"] = customer?.Username ?? "Unknown Customer";
                ViewData["FieldId"] = booking.FieldId;
                ViewData["CustomerId"] = booking.CustomerId;

                return View(booking);
            }

            // Tính toán giá trị TotalPrice và TotalAmount
            var fieldDetails = await _context.Fields.FindAsync(booking.FieldId);
            booking.TotalPrice = fieldDetails.Price * booking.Duration;
            booking.TotalServicePrice = 0; // Dịch vụ bổ sung (nếu có)
            booking.TotalAmount = booking.TotalPrice + booking.TotalServicePrice;

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Điều hướng về trang danh sách
            }

            // Nếu ModelState không hợp lệ, re-load thông tin sân và khách hàng
            var fieldDetail = _context.Fields.Find(booking.FieldId);
            var customerDetail = _context.Customers.Find(booking.CustomerId);

            ViewData["FieldName"] = fieldDetail?.FieldName ?? "Unknown Field";
            ViewData["CustomerName"] = customerDetail?.Username ?? "Unknown Customer";
            ViewData["FieldId"] = booking.FieldId;
            ViewData["CustomerId"] = booking.CustomerId;

            return View(booking); // Trả về lại view với thông tin đã nhập
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
