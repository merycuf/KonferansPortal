using KonferansPortal.Data;
using KonferansPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public Task<IActionResult> EgitmenAta(Konferans konferans, Uye egitmen)
        {
            Egitmen newEgitmen = new Egitmen(egitmen);
            
            konferans.Egitmenler.Add(newEgitmen);

            return null;// RedirectToAction("Index", "Admin");
        }
    }
}
