using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KonferansPortal.Data;
using KonferansPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;
using System.Dynamic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Policy;
using System.Text.Json.Serialization;
using System.Text.Json;

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

        // GET: Konferans
        public async Task<IActionResult> Index()
        {
            return View(await _context.Konferanslar.ToListAsync());
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
        public async Task<IActionResult> Create(Konferans konferans, IFormFile file)
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

        // GET: Konferans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = await CheckClaims((int)id);
            var konferans = await _context.Konferanslar.Include(k => k.Katilimcilar).Include(k => k.Egitmenler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (result == null || konferans == null)
            {
                return NotFound();
            }

            return View(konferans);
        }

        [HttpGet]
        public async Task<IActionResult> KayitOl(int id)
        {
            var konferans = await _context.Konferanslar.Include(k => k.Katilimcilar).Include(konferans => konferans.OnKayitListe).ThenInclude(o => o.uye).FirstOrDefaultAsync(k => k.Id == id);

            var currentUser = await _context.Uyeler.Include(u => u.katilinanKonferanslar).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (currentUser != null)
            {
                if (konferans.Katilimcilar == null)
                {
                    konferans.Katilimcilar = new List<Uye>();
                }

                else if (konferans.Katilimcilar.Any(k => k.Email == currentUser.Name))
                {
                    return RedirectToAction("KonferansMainView", new { id = id });
                }
                OnKayit onKayit = new OnKayit
                {
                    uye = currentUser,
                    dekontFile = null,
                    isPaid = false,
                    isChecked = false,
                    konferans = konferans
                };
                konferans.OnKayitListe.Add(onKayit);
                if (currentUser.onKayitKonferanslar == null)
                {
                    currentUser.onKayitKonferanslar = new List<OnKayit>();
                }
                currentUser.onKayitKonferanslar.Add(onKayit);
                await _context.SaveChangesAsync();
                return View();
            }
            return RedirectToAction("Login", "Uye");
        }


        [HttpGet]
        [Authorize(Policy = "IsEgitmenOrKatilimci")]
        public async Task<IActionResult> KonferansMainView(int id)
        {
            
            var konferans = await _context.Konferanslar.Include(k => k.Paylasimlar).FirstOrDefaultAsync(k => k.Id == id);

            if (konferans == null)
            {
                return NotFound();
            }
            

            return View(konferans);
        }

        [HttpGet]
        [Authorize(Policy = "IsEgitmenOrKatilimci")]
        public async Task<IActionResult> KatilimcilarListView(int id)
        {
            var result = await _context.Konferanslar
                .Include(k => k.Katilimcilar)
                .Include(k => k.Egitmenler)
                .ThenInclude(e => e.UyeModel)
                .FirstOrDefaultAsync(k => k.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }


        

        [Authorize(Policy = "IsEgitmen")]
        public IActionResult PaylasimEkle(int id)
        {
            return View();
        }
        [Authorize(Policy = "IsEgitmen")]
        [HttpPost]
        public async Task<IActionResult> PaylasimEkle(int id, string Title, string? Content, IFormFile file)
        {
            var paylasim = new Paylasim
            {
                Title = Title,
                Content = Content,
                ContentFile = file != null ? GetByteArrayFromFile(file) : null,
                Date = DateTime.UtcNow,
                Publisher = _context.Uyeler.FirstOrDefault(u => u.Email == User.Identity.Name),
                PaylasilanKonferans = await _context.Konferanslar.FirstAsync(k => k.Id == id)
            };
            var result = await _context.Konferanslar.Include(k => k.Paylasimlar).FirstAsync(k => k.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            if (result.Paylasimlar == null)
            {
                result.Paylasimlar = new List<Paylasim>();
            }
            result.Paylasimlar.Add(paylasim);
            _context.Konferanslar.Update(result);

            await _context.SaveChangesAsync();
            await CheckClaims(id);
            return RedirectToAction("KonferansMainView", new { id = id });
        }
        private byte[] GetByteArrayFromFile(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }

        /*public void dosyaIndir()
        {

        }*/
        [HttpGet]
        public IActionResult CreateTartisma(int id)
        {
            var model = new TartismaViewModel
            {
                KonferansId = id
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
                var konferans = await _context.Konferanslar.Include(k => k.Tartismalar).FirstOrDefaultAsync(k => k.Id == model.KonferansId);
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
                    Id = null
                };

                var result = _context.Add(tartisma);
                await _context.SaveChangesAsync();
                var konferanss = await _context.Konferanslar.Include(k => k.Tartismalar).FirstOrDefaultAsync(k => k.Id == model.KonferansId);
                if (konferanss.Tartismalar == null)
                {
                    konferanss.Tartismalar = new List<Tartisma>();
                }
                konferanss.Tartismalar.Add(result.Entity);
                _context.Konferanslar.Update(konferanss);
                await _context.SaveChangesAsync();
                return RedirectToAction("Tartismalar", new { konferansId = model.KonferansId });
            }
            return View(model);
        }
   

        [HttpGet]
        [Authorize(Policy = "IsEgitmenOrKatilimci")]
        public async Task<IActionResult> Tartismalar(int id)
        {
            Konferans? result = await _context.Konferanslar.Include(k=>k.Tartismalar).FirstOrDefaultAsync(k=> k.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            dynamic mymodel = new ExpandoObject();
            mymodel.KonferansId = id;
            mymodel.Tartismalar = result.Tartismalar;
            return View(mymodel);
        }
        [HttpGet]
        public async Task<IActionResult> TartismaDetails(int konferansId, int id)
        {
            var konferans = await _context.Konferanslar.Include(k => k.Tartismalar).FirstOrDefaultAsync(k => k.Id == konferansId);
            if(konferans == null || konferans.Tartismalar == null)
            {
                return NotFound();
            }
            Tartisma tartisma = konferans.Tartismalar.FirstOrDefault(t => t.Id == id);

            if (tartisma == null)
            {
                return NotFound();
            }
            return View(tartisma);
        }

        //

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

        [HttpGet]
        public async Task<IActionResult> GetOnayBekleyenler(int konferansId)
        {
            var konferans = await _context.Konferanslar.Include(k=> k.OnKayitListe).ThenInclude(o => o.uye).FirstOrDefaultAsync(k => k.Id == konferansId);
            var uyeList = konferans.OnKayitListe.Select(o => o.uye).ToList();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            var json = JsonSerializer.Serialize(uyeList, options);
            return Content(json, "application/json");
            
        }

        public async Task<FileResult> DownloadDekont(int konferansId, int kayitId)
        {
            var konferans = await _context.Konferanslar.Include(k => k.OnKayitListe).FirstOrDefaultAsync(k => k.Id == konferansId);
            if (konferans != null && konferans.OnKayitListe != null)
            {
                var uye = konferans.OnKayitListe.First(u => u.Id == kayitId);

                if (uye != null && uye.dekontFile != null)
                {
                    return File(uye.dekontFile, "application/octet-stream", "dekont${uyeId}.pdf");
                }
            }

            return null;
        }

        [HttpPost]
        public async Task<IActionResult> OnKayitOnayla(int konferansId, int kayitId)
        {
            var konferans = await _context.Konferanslar.Include(k => k.Katilimcilar)
                .Include(k => k.OnKayitListe).ThenInclude(o => o.uye).FirstOrDefaultAsync(k => k.Id == konferansId);
            if(konferans.Capacity <= konferans.Katilimcilar.Count)
            {
                
                return Json(false);
            }
            var uye = konferans.OnKayitListe.First(u => u.Id == kayitId);

            konferans.Katilimcilar.Add(uye.uye);
            konferans.OnKayitListe.Remove(uye);
            await _context.SaveChangesAsync();
            return Json(konferans?.OnKayitListe);
        }

        [HttpPost]
        public async Task<IActionResult> OnKayitReddet(int konferansId)
        {
            var konferans = await _context.Konferanslar.FirstOrDefaultAsync(k => k.Id == konferansId);
            return Json(konferans?.OnKayitListe);
        }

        private bool KonferansExists(int id)
        {
            return _context.Konferanslar.Any(e => e.Id == id);
        }

        public async Task<Konferans> CheckClaims(int id)
        {

            var konferans = await _context.Konferanslar.Include(k => k.Katilimcilar).Include(k => k.Egitmenler).Include(k => k.OnKayitListe).ThenInclude(o => o.uye)
                .FirstOrDefaultAsync(m => m.Id == id);

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (currentUser != null)
            {
                var claims = await _userManager.GetClaimsAsync(currentUser);
                var isEgitmenOrKatilimciClaim = claims.FirstOrDefault(c => c.Type == "IsEgitmenOrKatilimci");
                var isKatilimciClaim = claims.FirstOrDefault(c => c.Type == "IsKatilimci");
                var isEgitmenClaim = claims.FirstOrDefault(c => c.Type == "IsEgitmen");
                var isOnKayitliClaim = claims.FirstOrDefault(c => c.Type == "IsOnKayitli");

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
                if (isOnKayitliClaim != null)
                {
                    await _userManager.RemoveClaimAsync(currentUser, isOnKayitliClaim);
                }

                if (konferans.Egitmenler != null && konferans.Egitmenler.FirstOrDefault(e => e.UyeModel == currentUser) != null)
                    boolArray[0] = true;
                else
                    boolArray[0] = false;

                if (konferans.Katilimcilar != null && konferans.Katilimcilar.Contains(currentUser))
                    boolArray[1] = true;
                else
                    boolArray[1] = false;

                if (konferans.OnKayitListe != null && konferans.OnKayitListe.Find(o => o.uye.UserName == currentUser.Email) != null)
                {
                    await _userManager.AddClaimAsync(currentUser, new Claim("IsOnKayitli", "true"));
                }

                await _userManager.AddClaimsAsync(currentUser, new List<Claim>
                {
                    new Claim("IsEgitmenOrKatilimci", (boolArray[0] || boolArray[1]).ToString()),
                    new Claim("IsEgitmen", boolArray[0].ToString()),
                    new Claim("IsKatilimci", boolArray[1].ToString())
                });
            }

            return konferans;
        }

    }
}
