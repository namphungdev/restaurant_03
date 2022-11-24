using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurant.Helpers;
using Restaurant.Models;

namespace Restaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamsController : Controller
    {
        private readonly RestaurantContext _context;

        public SanPhamsController(RestaurantContext context)
        {
            _context = context;
        }

        private Boolean isExist()
        {
            if (SessionHelper.GetObjectFromJson<List<Models.Admin>>(HttpContext.Session, "loginadmin") != null && SessionHelper.GetObjectFromJson<List<Models.Admin>>(HttpContext.Session, "loginadmin").Count() > 0)
            {
                return true;
            }
            return false;
        }
        // GET: Admin/SanPhams
        public async Task<IActionResult> Index()
        {
            if (isExist())
            {
                var restaurantContext = _context.SanPhams.Include(s => s.MaLoaiSanPhamNavigation).Include(s => s.MaThucDonNavigation);
                return View(await restaurantContext.ToListAsync());
            }
            return RedirectToAction("Login", "Home");
           
        }

        // GET: Admin/SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (isExist())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var sanPham = await _context.SanPhams
                    .Include(s => s.MaLoaiSanPhamNavigation)
                    .Include(s => s.MaThucDonNavigation)
                    .FirstOrDefaultAsync(m => m.MaSanPham == id);
                if (sanPham == null)
                {
                    return NotFound();
                }

                return View(sanPham);
            }
            return RedirectToAction("Login", "Home");
           
        }

        // GET: Admin/SanPhams/Create
        public IActionResult Create()
        {
            if (isExist())
            {
                ViewData["MaLoaiSanPham"] = new SelectList(_context.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham");
                ViewData["MaThucDon"] = new SelectList(_context.ThucDons, "MaThucDon", "TenThucDon");
                return View();
            }
            return RedirectToAction("Login", "Home");
           
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSanPham,AnhSanPham,TenSanPham,ChiTiet,Tien,GiamGia,KichThuoc ,NgayNhap,NgayCapNhat,MaLoaiSanPham")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoaiSanPham"] = new SelectList(_context.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham", sanPham.MaLoaiSanPham);
            ViewData["MaThucDon"] = new SelectList(_context.ThucDons, "MaThucDon", "TenThucDon", sanPham.MaThucDon);
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (isExist())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var sanPham = await _context.SanPhams.FindAsync(id);
                if (sanPham == null)
                {
                    return NotFound();
                }
                ViewData["MaLoaiSanPham"] = new SelectList(_context.LoaiSanPhams, "MaLoaiSanPham", "TenLoaiSanPham", sanPham.MaLoaiSanPham);
                ViewData["MaThucDon"] = new SelectList(_context.ThucDons, "MaThucDon", "MaThucDon", sanPham.MaThucDon);
                return View(sanPham);
                return View(sanPham);
            }
            return RedirectToAction("Login", "Home");
          
        }

        // POST: Admin/SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSanPham,AnhSanPham,TenSanPham,NguyenLieu,ChiTiet,Tien,GiamGia,KichThuoc,SoLuongSanPham,NgayNhap,NgayCapNhat,MaLoaiSanPham,MaThucDon")] SanPham sanPham)
        {
            if (id != sanPham.MaSanPham)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.MaSanPham))
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
            ViewData["MaLoaiSanPham"] = new SelectList(_context.LoaiSanPhams, "MaLoaiSanPham", "MaLoaiSanPham", sanPham.MaLoaiSanPham);
            ViewData["MaThucDon"] = new SelectList(_context.ThucDons, "MaThucDon", "MaThucDon", sanPham.MaThucDon);
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (isExist())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var sanPham = await _context.SanPhams
                    .Include(s => s.MaLoaiSanPhamNavigation)
                    .Include(s => s.MaThucDonNavigation)
                    .FirstOrDefaultAsync(m => m.MaSanPham == id);
                if (sanPham == null)
                {
                    return NotFound();
                }

                return View(sanPham);
            }
            return RedirectToAction("Login", "Home");
           
        }

        // POST: Admin/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            _context.SanPhams.Remove(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.MaSanPham == id);
        }
    }
}
