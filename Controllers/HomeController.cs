﻿using FootballFieldManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FootballFieldManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FootballFieldManagementContext _context;
        public HomeController(ILogger<HomeController> logger, FootballFieldManagementContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy danh sách 4 sân bóng mới nhất từ cơ sở dữ liệu
            var fields = _context.Fields
                                 .OrderByDescending(f => f.FieldId) // Sắp xếp theo ngày tạo mới nhất
                                 .Take(4) // Lấy 4 sân bóng mới nhất
                                 .ToList();

            return View(fields);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
