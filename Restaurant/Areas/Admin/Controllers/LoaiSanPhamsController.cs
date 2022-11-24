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
    public class LoaiSanPhamsController : Controller
    {
        private readonly RestaurantContext _context;

        public LoaiSanPhamsController(RestaurantContext context)
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
        // GET: Admin/LoaiSanPhams
        public async Task<IActionResult> Index()
        {
            if (isExist())
            {
                return View(await _context.LoaiSanPhams.ToListAsync());
            }
            return RedirectToAction("Login", "Home");
           
        }

        // GET: Admin/LoaiSanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (isExist())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var loaiSanPham = await _context.LoaiSanPhams
                    .FirstOrDefaultAsync(m => m.MaLoaiSanPham == id);
                if (loaiSanPham == null)
                {
                    return NotFound();
                }

                return View(loaiSanPham);
            }
            return RedirectToAction("Login", "Home");
          
        }

        // GET: Admin/LoaiSanPhams/Create
        public IActionResult Create()
        {
            if (isExist())
            {
                return View();
            }
            return RedirectToAction("Login", "Home");
           
        }

        // POST: Admin/LoaiSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoaiSanPham,TenLoaiSanPham")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiSanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiSanPham);
        }

        // GET: Admin/LoaiSanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (isExist())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
                if (loaiSanPham == null)
                {
                    return NotFound();
                }
                return View(loaiSanPham);
            }
            return RedirectToAction("Login", "Home");
           
        }

        // POST: Admin/LoaiSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLoaiSanPham,TenLoaiSanPham")] LoaiSanPham loaiSanPham)
        {
            if (id != loaiSanPham.MaLoaiSanPham)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiSanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiSanPhamExists(loaiSanPham.MaLoaiSanPham))
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
            return View(loaiSanPham);
        }

        // GET: Admin/LoaiSanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (isExist())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var loaiSanPham = await _context.LoaiSanPhams
                    .FirstOrDefaultAsync(m => m.MaLoaiSanPham == id);
                if (loaiSanPham == null)
                {
                    return NotFound();
                }

                return View(loaiSanPham);
            }
            return RedirectToAction("Login", "Home");
            
        }

        // POST: Admin/LoaiSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            _context.LoaiSanPhams.Remove(loaiSanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiSanPhamExists(int id)
        {
            return _context.LoaiSanPhams.Any(e => e.MaLoaiSanPham == id);
        }
    }
}
