using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KonferansPortal.Data;
using KonferansPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;

namespace KonferansPortal.Controllers
{
    public class KonferansController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<Uye> _userManager;

        public KonferansController(AppDbContext context, IAuthorizationService authorizationService, UserManager<Uye> userManager)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Policy = "IsEgitmenOrKatilimci")]
        public async Task<IActionResult> KonferansMainView(int id)
        {
            /*var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, new IsKatilimciRequirement());//patlıyo

            if (!authorizationResult.Succeeded)
            {
                authorizationResult = await _authorizationService.AuthorizeAsync(User, null, new IsEgitmenRequirement());
                if (!authorizationResult.Succeeded)
                    return Forbid();
            }*/

            var konferans = await _context.Konferanslar.FirstOrDefaultAsync(k => k.Id == id);

            if (konferans == null)
            {
                return NotFound();
            }

            return View(konferans);
        }

        // GET: Konferans
        public async Task<IActionResult> Index()
        {
            return View(await _context.Konferanslar.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> KayitOl(int id)
        {
            var konferans = await _context.Konferanslar.Include(k => k.Katilimcilar)
                .FirstOrDefaultAsync(m => m.Id == id);
            var currentUser = await _context.Uyeler.Include(u => u.katilinanKonferanslar).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (currentUser != null)
            {
                if (konferans.Katilimcilar == null)
                {
                    konferans.Katilimcilar = new List<Uye>();
                }

                else if (konferans.Katilimcilar.Any(k => k.Email == currentUser.Name))
                {
                    return RedirectToAction("KonferansMainView", "Konferans");
                }

                konferans.Katilimcilar.Add(currentUser);
                if (currentUser.katilinanKonferanslar == null)
                {
                    currentUser.katilinanKonferanslar = new List<Konferans>();
                }
                currentUser.katilinanKonferanslar.Add(konferans);
                await _context.SaveChangesAsync();
                return View();
            }
            return RedirectToAction("Login", "Uye");
        }

        // GET: Konferans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konferans = await _context.Konferanslar.Include(k => k.Katilimcilar).Include(k => k.Egitmenler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (konferans == null)
            {
                return NotFound();
            }

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (currentUser != null)
            {
                var claims = await _userManager.GetClaimsAsync(currentUser);
                var isEgitmenOrKatilimciClaim = claims.FirstOrDefault(c => c.Type == "IsEgitmenOrKatilimci");
                var isKatilimciClaim = claims.FirstOrDefault(c => c.Type == "IsKatilimci");
                var isEgitmenClaim = claims.FirstOrDefault(c => c.Type == "IsEgitmen");

                bool[] boolArray = [false, false];
                if (isEgitmenOrKatilimciClaim != null)
                {
                    await _userManager.RemoveClaimAsync(currentUser, isEgitmenOrKatilimciClaim);
                }
                if (isKatilimciClaim != null)
                {
                    await _userManager.RemoveClaimAsync(currentUser, isKatilimciClaim);
                }
                if (isEgitmenClaim != null)
                {
                    await _userManager.RemoveClaimAsync(currentUser, isEgitmenClaim);
                }

                if (konferans.Egitmenler!= null && konferans.Egitmenler.FirstOrDefault(e=>e.Name == currentUser.Name) != null)
                    boolArray[0] = true;
                else
                    boolArray[0] = false;

                if (konferans.Katilimcilar!= null && konferans.Katilimcilar.Contains(currentUser))
                    boolArray[1] = true;
                else
                    boolArray[1] = false;

                await _userManager.AddClaimsAsync(currentUser, new List<Claim>
                {
                    new Claim("IsEgitmenOrKatilimci", (boolArray[0] || boolArray[1]).ToString()),
                    new Claim("IsEgitmen", boolArray[0].ToString()),
                    new Claim("IsKatilimci", boolArray[1].ToString())
                });


            }
            return View(konferans);
        }

        
        // GET: Konferans/Create
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Konferans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Konferans konferans,IFormFile file)
        {
            konferans.KonferansImage = file != null ? GetByteArrayFromFile(file) : null;
            if (ModelState.IsValid)
            {
                _context.Add(konferans);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(konferans);
        }

        // GET: Konferans/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [Authorize(Policy = "IsEgitmen")]
        public IActionResult PaylasimEkle(int id)
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "IsEgitmen")]
        /*public async Task<IActionResult> PaylasimEkle(IFormFile File)
        {
            if (File != null)
            {
                var extent = Path.GetExtension(File.FileName);
                var randomName = ($"{Guid.NewGuid()}{extent}");
                //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await File.CopyToAsync(stream);
                }
            }
            return View();
        }*/
        public void dosyaIndir()
        {

        }
        [HttpGet]
        public IActionResult CreateTartisma(int konferansId)
        {
            var model = new TartismaViewModel
            {
                KonferansId = konferansId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTartisma(TartismaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var konferans = await _context.Konferanslar.FindAsync(model.KonferansId);

                if (konferans == null)
                {
                    return NotFound();
                }

                var tartisma = new Tartisma
                {
                    Title = model.Title,
                    Content = model.Content,
                    Date = DateTime.Now,
                    Publisher = currentUser,
                    Konferans = konferans,
                    Yorumlar = new List<Yorum>()
                };

                konferans.Tartismalar.Add(tartisma);
                _context.Konferanslar.Update(konferans);
                await _context.SaveChangesAsync();
                return RedirectToAction("Tartismalar", new { konferansId = model.KonferansId });
            }
            return View(model);
        }
   

        [HttpGet]
        [Authorize(Policy = "IsEgitmenOrKatilimci")]
        public async Task<IActionResult> Tartismalar(int id)
        {
            Konferans? result = await _context.Konferanslar.FirstOrDefaultAsync(k=> k.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result.Tartismalar);
        }

        [HttpGet]
        [Authorize(Policy = "IsEgitmenOrKatilimci")]
        public async Task<IActionResult> KatilimcilarListView(int id)
        {
            var result = await _context.Konferanslar
                .Include(k => k.Katilimcilar)
                .Include(k => k.Egitmenler).FirstOrDefaultAsync(k => k.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }


        private byte[] GetByteArrayFromFile(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }

        [HttpPost]
        [Authorize(Policy = "IsEgitmen")]
        public async Task<IActionResult> PaylasimEkle(int id,string Title,string? Content,IFormFile file)
        {
           var paylasim = new Paylasim
            {
                Title = Title,
                Content = Content,
                ContentFile = file != null ? GetByteArrayFromFile(file) : null,
                // Set other properties as needed
                Date = DateTime.UtcNow,
                Publisher = _context.Uyeler.FirstOrDefault(u => u.Name == User.Identity.Name)
            };
            var result = await _context.Konferanslar.Include(k=> k.Paylasimlar).FirstAsync(k=> k.Id == id);
            if(result == null)
            {
                return NotFound();
            }
            if(result.Paylasimlar == null)
            {
                result.Paylasimlar = new List<Paylasim>();
            }
            result.Paylasimlar.Add(paylasim);
            _context.Konferanslar.Update(result);
            await  _context.SaveChangesAsync();
            return RedirectToAction("KonferansMainView", id);
        }

    }
}
