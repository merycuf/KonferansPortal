using KonferansPortal.Data;
using KonferansPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KonferansPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var totalDuyurular = await _context.Duyurular.CountAsync();
            var totalPages = (int)Math.Ceiling(totalDuyurular / (double)pageSize);

            var duyurular = await _context.Duyurular
                .OrderByDescending(d => d.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new PagedDuyuruViewModel
            {
                Duyurular = duyurular,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ContactUs() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(string Name, string Email, string Message, string Subject)
        {
            ContactMessage contactMessage = new ContactMessage
            {
                Email = Email,
                Message = Message,
                Name = Name,
                Subject = Subject
            };
            
            _context.ContactMessages.Add(contactMessage);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
