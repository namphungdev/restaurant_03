using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Controllers
{
    public class ProductController : Controller
    {

        private readonly RestaurantContext _context;

        public ProductController( RestaurantContext context      )
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var listsanpham = _context.SanPhams.ToList();
            return View(listsanpham);

        }

       /* public IActionResult Index(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 10;
                var listSanpham = _context.SanPhams.AsNoTracking().OrderByDescending(x => x.MaSanPham);
                PagedList<SanPham> models = new PagedList<SanPham>(listSanpham, pageSize, pageNumber);

                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", " Home");
            }


        }*/
        public IActionResult List(int MaLoaiSanPham, int page = 1)
        {
            try
            {
                var pageSize = 10;
                var DanhMuc = _context.LoaiSanPhams.Find(MaLoaiSanPham);
                var listSanpham = _context.SanPhams.AsNoTracking().Where(x => x.MaLoaiSanPham == MaLoaiSanPham).OrderByDescending(x => x.MaSanPham);
                PagedList<SanPham> models = new PagedList<SanPham>(listSanpham, pageSize, page);

                ViewBag.CurrentPage = page;
                ViewBag.CurrentLoaiSanPham = DanhMuc;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", " Home");
            }
            

        }
        //public IActionResult Details()
        //{
        //    return View();
        //}

        public IActionResult Details(int id)
        {
            var product = _context.SanPhams.FirstOrDefault(p => p.MaSanPham == id);

            return View(product);
        }
    }
}
