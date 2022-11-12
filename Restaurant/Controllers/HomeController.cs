using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Restaurant.Helpers;
using Restaurant.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Controllers
{
    public class HomeController : Controller
    {
      /*  private readonly ILogger<HomeController> _logger;*/
        private readonly RestaurantContext _context;
        //private readonly  GioHangService _gioHangService;

        public HomeController(RestaurantContext context)
        {
            _context = context;
            //_gioHangService = gioHangService;
        }
       /* public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
          
        }*/
        public async Task<IActionResult> Index()
        {
            /* var sanpham = _context.SanPhams.OrderBy(x => Guid.NewGuid()).Take(8);*/
            var sanpham = _context.SanPhams.Include(l => l.MaLoaiSanPhamNavigation).OrderBy(x => Guid.NewGuid()).Take(8);

            return View(sanpham);          
        }

        public IActionResult Blog()
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
