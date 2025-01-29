﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KonferansPortal.Data;
using KonferansPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
        [Authorize(Policy = "IsKatilimci")]
        [Authorize(Policy = "IsEgitmen")]
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
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if(currentUser != null)
            {
                if (konferans.Katilimcilar == null)
                {
                    konferans.Katilimcilar = new List<Uye>();
                }

                else if (konferans.Katilimcilar.Any(k => k.Id == currentUser.Id))
                {
                    return RedirectToAction("KonferansMainView", "Konferans");
                }

                konferans.Katilimcilar.Add(currentUser);
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

            var konferans = await _context.Konferanslar.Include(k => k.Katilimcilar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (konferans == null)
            {
                return NotFound();
            }

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (currentUser != null)
            {
                var claims = await _userManager.GetClaimsAsync(currentUser);
                var isKatilimciClaim = claims.FirstOrDefault(c => c.Type == "IsKatilimci");
                if (isKatilimciClaim != null)
                {
                    await _userManager.RemoveClaimAsync(currentUser, isKatilimciClaim);
                }
                if (konferans.Katilimcilar == null)
                {
                    konferans.Katilimcilar = new List<Uye>();
                }
                if (konferans.Katilimcilar.Any(k => k.Id == currentUser.Id))
                {
                    await _userManager.AddClaimAsync(currentUser, new Claim("IsKatilimci", "true"));
                }
                else
                {
                    await _userManager.AddClaimAsync(currentUser, new Claim("IsKatilimci", "false"));
                }
            }
            return View(konferans);
        }

        // POST: Konferans/Details/5
        [HttpPost]
        public async Task<IActionResult> Details(Uye model, int? id)
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
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (currentUser != null)
            {
                if (konferans.Egitmenler.Contains(currentUser))
                {
                    var result = await _userManager.AddClaimAsync(currentUser, new Claim("IsEgitmen", "true"));
                    return RedirectToAction("KonferansMainView", "Konferans", konferans.Id);
                }
                else
                {
                    var result = await _userManager.AddClaimAsync(currentUser, new Claim("IsEgitmen", "false"));
                }

                if (konferans.Katilimcilar.Contains(currentUser))
                {
                    var result = await _userManager.AddClaimAsync(currentUser, new Claim("IsKatilimci", "true"));
                    return RedirectToAction("KonferansMainView", "Konferans", konferans.Id);
                }
                else
                {
                    var result = await _userManager.AddClaimAsync(currentUser, new Claim("IsKatilimci", "false"));
                }
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

        [HttpPost]
        public async Task<IActionResult> PaylasimEkle(IFormFile File)
        {
            if (File != null)
            {
                var extent = Path.GetExtension(File.FileName);
                var randomName = ($"{Guid.NewGuid()}{extent}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await File.CopyToAsync(stream);
                }
            }
            return View();
        }
        public void dosyaIndir() {

        }

        [HttpGet]
        public async Task<IActionResult> Tartismalar(int id)
        {
            Konferans? result = await _context.Konferanslar.FindAsync(id);
            if(result == null)
            {
                return NotFound();
            }
            return View(result.Tartismalar);
        }

        [HttpGet]
        [Authorize(Policy = "IsKatilimci")]
        [Authorize(Policy = "IsEgitmen")]
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

    }
    

}
