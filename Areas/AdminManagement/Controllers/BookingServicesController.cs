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
            var footballFieldManagementContext = _context.BookingServices.Include(b => b.Booking).Include(b => b.Service);
            return View(await footballFieldManagementContext.ToListAsync());
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
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId");
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId");
            return View();
        }

        // POST: AdminManagement/BookingServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingServiceId,BookingId,ServiceId,Quantity,TotalPrice")] BookingService bookingService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookingService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", bookingService.BookingId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", bookingService.ServiceId);
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
                    _context.Update(bookingService);
                    await _context.SaveChangesAsync();
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
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", bookingService.ServiceId);
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
