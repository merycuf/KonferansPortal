using KonferansPortal.Data;
using KonferansPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace KonferansPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EgitmenAta()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Konferanslar = _context.Konferanslar.ToArray();
            mymodel.Egitmenler = _context.Uyeler.ToArray();
            return View(mymodel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EgitmenAta(int konferansId, string egitmenEmail)
        {
            var konferansContext = await _context.Konferanslar
            .Include(k => k.Egitmenler)
            .FirstOrDefaultAsync(k => k.Id == konferansId);

            var contextUye = await _context.Uyeler
            .FirstOrDefaultAsync(e => e.Email == egitmenEmail);

            Egitmen newEgitmen = new Egitmen();
            newEgitmen = newEgitmen.fillEgitmen(contextUye);

            if(konferansContext.Egitmenler == null)
            {
                konferansContext.Egitmenler = new List<Egitmen>();
            }
            konferansContext.Egitmenler.Add(newEgitmen);

            if(newEgitmen.EgitilenKonferans==null)
                newEgitmen.EgitilenKonferans = new List<Konferans>();

            newEgitmen.EgitilenKonferans.Add(konferansContext);

            _context.Egitmenler.Add(newEgitmen);
            _context.Konferanslar.Update(konferansContext);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");
        }
    }
}
