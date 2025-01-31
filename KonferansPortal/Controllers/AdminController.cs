using KonferansPortal.Data;
using KonferansPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace KonferansPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Uye> userManager;
        public AdminController(AppDbContext context, UserManager<Uye> userManager)
        {
            _context = context;
            this.userManager = userManager;
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
            var result = await _context.Egitmenler
                .Include(e => e.UyeModel)
                .Include(e => e.EgitilenKonferans)
                .FirstOrDefaultAsync(e => e.UyeId == contextUye.Email);
            if (result == null)
            {
                Egitmen newEgitmen = new Egitmen
                {
                    UyeModel = contextUye,
                    EgitilenKonferans = new List<Konferans>()
                };
                await _context.Egitmenler.AddAsync(newEgitmen);
                await _context.SaveChangesAsync();
                result = await _context.Egitmenler.FirstOrDefaultAsync(e => e.UyeModel.Email == egitmenEmail);
            }
            if (result.EgitilenKonferans == null)
                result.EgitilenKonferans = new List<Konferans>();

            result.EgitilenKonferans.Add(konferansContext);

            if (konferansContext.Egitmenler == null)
            {
                konferansContext.Egitmenler = new List<Egitmen>();
            }
            konferansContext.Egitmenler.Add(result);
            _context.Konferanslar.Update(konferansContext);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminAta()
        {
            return View(await _context.Uyeler.ToListAsync());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AdminAta(string uyeMail)
        {


            var contextUye = await _context.Uyeler
            .FirstOrDefaultAsync(e => e.Email == uyeMail);

            if (contextUye == null)
            {
                return NotFound();
            }
            
            contextUye.Discriminator = "Admin";
            var res = await userManager.AddToRoleAsync(contextUye, "Admin");

            _context.Uyeler.Update(contextUye);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");
        }

    }
}
