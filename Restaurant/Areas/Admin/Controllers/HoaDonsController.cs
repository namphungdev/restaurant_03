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
    public class HoaDonsController : Controller
    {
        private readonly RestaurantContext _context;

        public HoaDonsController(RestaurantContext context)
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
        // GET: Admin/HoaDons
        public async Task<IActionResult> Index()
        {          
            if (isExist())
            {
                var restaurantContext = _context.HoaDons.Include(h => h.MaThanhToanNavigation).Include(i => i.MaVanChuyenNavigation);
                return View(await restaurantContext.ToListAsync());
            }
            return RedirectToAction("Login", "Home");

           
        }

        // GET: Admin/HoaDons/Details/5
        public async Task<IActionResult> Details(int? id)
        {         
            if (isExist())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var hoaDon = await _context.HoaDons
                    .Include(h => h.MaKhachHangNavigation)
                    .Include(i => i.MaThanhToanNavigation)
                    .Include(u => u.MaVanChuyenNavigation)
                    .FirstOrDefaultAsync(m => m.MaHoaDon == id);
                if (hoaDon == null)
                {
                    return NotFound();
                }

                return View(hoaDon);
            }
            return RedirectToAction("Login", "Home");
           
        }

        // GET: Admin/HoaDons/Create
        public IActionResult Create()
        {
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "DiaChi");
            return View();
        }

        // POST: Admin/HoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHoaDon,MaKhachHang,NgayLap,TongTien,SoLuong,DiaChi")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "DiaChi", hoaDon.MaKhachHang);
            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (isExist())
            {
                if (id == null)
                {
                    return NotFound();
                }

                /*var hoaDon = await _context.HoaDons.FindAsync(id);*/
                var hoaDon = await _context.HoaDons
                   .Include(h => h.MaKhachHangNavigation)
                   .Include(i => i.MaThanhToanNavigation)
                   .Include(u => u.MaVanChuyenNavigation)
                   .FirstOrDefaultAsync(m => m.MaHoaDon == id);
                var cthd = _context.ChiTietHoaDons
                    .Include(c => c.MaSanPhamNavigation)
                    .Include(c => c.MaHoaDonNavigation)
                    .Where(m => m.MaHoaDon == id).ToList();
                if (hoaDon == null)
                {
                    return NotFound();
                }
                ViewData["cthd"] = cthd;
                ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhacHang", "TenKhachHang", hoaDon.MaKhachHang);
                ViewData["MaThanhToan"] = new SelectList(_context.ThanhToans, "MaThanhToan", "TrangThaiThanhToan", hoaDon.MaThanhToan);
                ViewData["MaVanChuyen"] = new SelectList(_context.VanChuyens, "MaVanChuyen", "TrangThaiVanChuyen", hoaDon.MaVanChuyen);
                return View(hoaDon);
            }
            return RedirectToAction("Login", "Home");
           
        }

        // POST: Admin/HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHoaDon,MaKhachHang,NgayLap,TongTien,SoLuong,DiaChi,Sdt,MaThanhToan,MaVanChuyen")] HoaDon hoaDon)
        {
            if (id != hoaDon.MaHoaDon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonExists(hoaDon.MaHoaDon))
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
            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (isExist())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var hoaDon = await _context.HoaDons
                    .Include(h => h.MaKhachHangNavigation)
                    .Include(i => i.MaThanhToanNavigation)
                    .Include(u => u.MaVanChuyenNavigation)
                    .FirstOrDefaultAsync(m => m.MaHoaDon == id);
                if (hoaDon == null)
                {
                    return NotFound();
                }

                return View(hoaDon);
            }
            return RedirectToAction("Login", "Home");
            
        }

        // POST: Admin/HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            var cthd = await _context.ChiTietHoaDons.FirstOrDefaultAsync(m => m.MaHoaDon == id);
            _context.ChiTietHoaDons.Remove(cthd);
            _context.HoaDons.Remove(hoaDon);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(int id)
        {
            return _context.HoaDons.Any(e => e.MaHoaDon == id);
        }
    }
}
