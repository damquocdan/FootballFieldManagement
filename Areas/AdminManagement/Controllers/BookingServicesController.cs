using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballFieldManagement.Models;

namespace FootballFieldManagement.Areas.AdminManagement.Controllers
{
    [Area("AdminManagement")]
    public class BookingServicesController : Controller
    {
        private readonly FootballFieldManagementContext _context;

        public BookingServicesController(FootballFieldManagementContext context)
        {
            _context = context;
        }

        // GET: AdminManagement/BookingServices
        public async Task<IActionResult> Index()
        {
            // Bao gồm cả Booking và Customer để tránh null
            var footballFieldManagementContext = _context.BookingServices
                .Include(b => b.Booking)  // Bao gồm thông tin Booking
                .ThenInclude(b => b.Customer) // Bao gồm thông tin Customer trong Booking
                .Include(b => b.Service);  // Bao gồm thông tin Service

            var bookingServices = await footballFieldManagementContext.ToListAsync();

            return View(bookingServices);
        }



        // GET: AdminManagement/BookingServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingService = await _context.BookingServices
                .Include(b => b.Booking)
                .Include(b => b.Service)
                .FirstOrDefaultAsync(m => m.BookingServiceId == id);
            if (bookingService == null)
            {
                return NotFound();
            }

            return View(bookingService);
        }

        // GET: AdminManagement/BookingServices/Create
        public IActionResult Create()
        {
            // Lấy danh sách BookingId để hiển thị trong dropdown
            var bookings = _context.Bookings.Select(b => new SelectListItem
            {
                Value = b.BookingId.ToString(),
                Text = $"{b.BookingId} - {b.Customer.Username}"
            }).ToList();
            ViewBag.BookingId = bookings;

            // Lấy danh sách dịch vụ để hiển thị trong dropdown
            var services = _context.Services.Select(s => new {
                s.ServiceId,
                s.ServiceName,
                s.Price
            }).ToList();

            // Pass dữ liệu services để hiển thị trong dropdown và sử dụng trong JavaScript
            ViewBag.ServiceId = new SelectList(services, "ServiceId", "ServiceName");
            ViewBag.ServiceData = services; // Dữ liệu này sẽ được sử dụng trong JavaScript để tính TotalPrice

            return View();
        }


        // POST: AdminManagement/BookingServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingServiceId,BookingId,ServiceId,Quantity,TotalPrice")] BookingService bookingService)
        {
            // Kiểm tra tính hợp lệ của model
            if (ModelState.IsValid)
            {
                // Lấy dịch vụ từ cơ sở dữ liệu dựa trên ServiceId
                var service = await _context.Services
                    .FirstOrDefaultAsync(s => s.ServiceId == bookingService.ServiceId);

                // Nếu tìm thấy dịch vụ, tính toán giá tổng
                if (service != null)
                {
                    bookingService.TotalPrice = service.Price * bookingService.Quantity;
                }

                // Thêm bookingService vào cơ sở dữ liệu
                _context.Add(bookingService);
                await _context.SaveChangesAsync();

                // Cập nhật lại giá trị TotalServicePrice trong bảng Booking nếu có dịch vụ được thêm
                var booking = await _context.Bookings
                    .Include(b => b.BookingServices)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingService.BookingId);

                if (booking != null)
                {
                    // Tính lại TotalServicePrice của booking sau khi thêm dịch vụ
                    booking.TotalServicePrice = booking.BookingServices.Sum(bs => bs.TotalPrice);
                    booking.TotalAmount = booking.TotalPrice + booking.TotalServicePrice;

                    // Cập nhật thông tin vào cơ sở dữ liệu
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }

                // Redirect đến trang danh sách sau khi lưu thành công
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi trong model, truyền lại dữ liệu và chọn lại dropdown
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", bookingService.BookingId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceName", bookingService.ServiceId);

            return View(bookingService);
        }


        // GET: AdminManagement/BookingServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingService = await _context.BookingServices.FindAsync(id);
            if (bookingService == null)
            {
                return NotFound();
            }
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", bookingService.BookingId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", bookingService.ServiceId);
            return View(bookingService);
        }

        // POST: AdminManagement/BookingServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingServiceId,BookingId,ServiceId,Quantity,TotalPrice")] BookingService bookingService)
        {
            if (id != bookingService.BookingServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy dịch vụ từ cơ sở dữ liệu dựa trên ServiceId
                    var service = await _context.Services
                        .FirstOrDefaultAsync(s => s.ServiceId == bookingService.ServiceId);

                    // Nếu tìm thấy dịch vụ, tính toán lại giá tổng
                    if (service != null)
                    {
                        bookingService.TotalPrice = service.Price * bookingService.Quantity;
                    }

                    // Cập nhật bookingService vào cơ sở dữ liệu
                    _context.Update(bookingService);
                    await _context.SaveChangesAsync();

                    // Cập nhật lại giá trị TotalServicePrice trong bảng Booking sau khi chỉnh sửa dịch vụ
                    var booking = await _context.Bookings
                        .Include(b => b.BookingServices)
                        .FirstOrDefaultAsync(b => b.BookingId == bookingService.BookingId);

                    if (booking != null)
                    {
                        // Tính lại TotalServicePrice của booking sau khi chỉnh sửa dịch vụ
                        booking.TotalServicePrice = booking.BookingServices.Sum(bs => bs.TotalPrice);
                        booking.TotalAmount = booking.TotalPrice + booking.TotalServicePrice;

                        // Cập nhật thông tin vào cơ sở dữ liệu
                        _context.Update(booking);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingServiceExists(bookingService.BookingServiceId))
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

            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", bookingService.BookingId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceName", bookingService.ServiceId);
            return View(bookingService);
        }


        // GET: AdminManagement/BookingServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingService = await _context.BookingServices
                .Include(b => b.Booking)
                .Include(b => b.Service)
                .FirstOrDefaultAsync(m => m.BookingServiceId == id);
            if (bookingService == null)
            {
                return NotFound();
            }

            return View(bookingService);
        }

        // POST: AdminManagement/BookingServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookingService = await _context.BookingServices.FindAsync(id);
            if (bookingService != null)
            {
                _context.BookingServices.Remove(bookingService);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingServiceExists(int id)
        {
            return _context.BookingServices.Any(e => e.BookingServiceId == id);
        }
    }
}
