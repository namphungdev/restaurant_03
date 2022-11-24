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
    public class AdminsController : Controller
    {
        private readonly RestaurantContext _context;

        public AdminsController(RestaurantContext context)
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

        // GET: Admin/Admins
        public async Task<IActionResult> Index()
        {
            if (isExist())
            {
                return View(await _context.Admins.ToListAsync());
            }
            return RedirectToAction("Login", "Home");
           
        }

        // GET: Admin/Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (isExist())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var admin = await _context.Admins
                    .FirstOrDefaultAsync(m => m.MaTk == id);
                if (admin == null)
                {
                    return NotFound();
                }

                return View(admin);
            }
            return RedirectToAction("Login", "Home");
           
        }

        // GET: Admin/Admins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTk,Tk,Mk")] Models.Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // GET: Admin/Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (isExist())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var admin = await _context.Admins.FindAsync(id);
                if (admin == null)
                {
                    return NotFound();
                }
                return View(admin);
            }
            return RedirectToAction("Login", "Home");
           
        }

        // POST: Admin/Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTk,Tk,Mk")] Models.Admin admin)
        {
            if (id != admin.MaTk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.MaTk))
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
            return View(admin);
        }

        // GET: Admin/Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (isExist())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var admin = await _context.Admins
                    .FirstOrDefaultAsync(m => m.MaTk == id);
                if (admin == null)
                {
                    return NotFound();
                }

                return View(admin);
            }
            return RedirectToAction("Login", "Home");
            
        }

        // POST: Admin/Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.MaTk == id);
        }
    }
}
