using FootballFieldManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballFieldManagement.Areas.AdminManagement.Controllers {

    public class DashboardController : BaseController
    {
        private readonly FootballFieldManagementContext _context;

        public DashboardController(FootballFieldManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            return View();
        }
    }

}
