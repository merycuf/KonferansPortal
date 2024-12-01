using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KonferansPortal.Data;
using KonferansPortal.Models;

namespace KonferansPortal.Controllers
{
    public class KonferansController : Controller
    {
        private readonly AppDbContext _context;

        public KonferansController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Konferans
        public async Task<IActionResult> Index()
        {
            return View(await _context.Konferanslar.ToListAsync());
        }

        // GET: Konferans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konferans = await _context.Konferanslar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (konferans == null)
            {
                return NotFound();
            }

            return View(konferans);
        }

        // GET: Konferans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Konferans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,Description,Price,StartDate,EndDate,ImageUrl")] Konferans konferans)
        {
            if (ModelState.IsValid)
            {
                _context.Add(konferans);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(konferans);
        }

        // GET: Konferans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konferans = await _context.Konferanslar.FindAsync(id);
            if (konferans == null)
            {
                return NotFound();
            }
            return View(konferans);
        }

        // POST: Konferans1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,Description,Price,StartDate,EndDate,ImageUrl")] Konferans konferans)
        {
            if (id != konferans.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(konferans);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KonferansExists(konferans.Id))
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
            return View(konferans);
        }

        // GET: Konferans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konferans = await _context.Konferanslar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (konferans == null)
            {
                return NotFound();
            }

            return View(konferans);
        }

        // POST: Konferans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var konferans = await _context.Konferanslar.FindAsync(id);
            if (konferans != null)
            {
                _context.Konferanslar.Remove(konferans);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KonferansExists(int id)
        {
            return _context.Konferanslar.Any(e => e.Id == id);
        }
    }
}
