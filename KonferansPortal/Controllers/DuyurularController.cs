using KonferansPortal.Data;
using KonferansPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KonferansPortal.Controllers
{
    public class DuyurularController : Controller
    {
        private readonly AppDbContext _context;

        public DuyurularController(AppDbContext context)
        {
            _context = context;
        }
        // GET: DuyurularController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DuyurularController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _context.Duyurular.FirstOrDefaultAsync(d => d.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: DuyurularController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View(new Duyurular());
        }

        // POST: DuyurularController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Duyurular duyuru)
        {

            duyuru.Date = DateTime.Now;          

            _context.Add(duyuru);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
            
        }

        // GET: DuyurularController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DuyurularController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(id);
            }
        }

        // GET: DuyurularController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DuyurularController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
