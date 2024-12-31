using KonferansPortal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KonferansPortal.Data
{
    public class AppDbContext : IdentityDbContext<Uye>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<KonferansPortal.Models.Konferans> Konferanslar { get; set; }
        public DbSet<KonferansPortal.Models.Duyurular> Duyurular { get; set; }

        public DbSet<KonferansPortal.Models.Uye> Uyeler { get; set; }
        public DbSet<KonferansPortal.Models.Egitmen> Egitmenler { get; set; }
    }
}
