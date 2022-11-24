using Microsoft.AspNetCore.Mvc;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant.Helpers;

namespace Restaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly RestaurantContext _context;

        public HomeController(RestaurantContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {         
            if (isExist())
            {
                return View();
            }
            return RedirectToAction("Login", "Home");



        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult test()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Login(string tk, string mk)
        {
            string msg = "";
            TempData["ErrorMessage"] = msg;
            var ad = _context.Admins.ToList();
            var data = _context.Admins.Where(p => p.Tk == tk && p.Mk == mk).ToList();
            if (data.Count() > 0)
            {
                List<Models.Admin> admin = new List<Models.Admin>();
                admin.Add(new Models.Admin { Tk = tk });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "loginadmin", admin);
                return View("index");
                /* List<Login> admin = new List<Login>();
                 admin.Add(new Login { admin = ad.Find(p => p.Tk == tk) });
                 SessionHelper.SetObjectAsJson(HttpContext.Session, "login", admin);
                 return View("index");*/
            }
            else
            {
                msg = "Thông tin đăng nhập không chính xác. Vui lòng kiểm tra lại.";
                TempData["ErrorMessage"] = msg;
                return RedirectToAction("Login", "Home");

            }

        }
        public async Task<IActionResult> Logout()
        {
            SessionHelper.GetObjectFromJson<List<Models.Admin>>(HttpContext.Session, "loginadmin").Clear();
            List<Models.Admin> login = new List<Models.Admin>();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "loginadmin", login);
            return RedirectToAction("Login", "Home");
        }
        private Boolean isExist()
        {
            if (SessionHelper.GetObjectFromJson<List<Models.Admin>>(HttpContext.Session, "loginadmin") != null && SessionHelper.GetObjectFromJson<List<Models.Admin>>(HttpContext.Session, "loginadmin").Count() > 0)
            {              
                return true;
            }
            return false;
        }
    }
}
