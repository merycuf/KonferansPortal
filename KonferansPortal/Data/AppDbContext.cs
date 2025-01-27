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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the many-to-many relationship between Egitmen and Konferans
            modelBuilder.Entity<Egitmen>()
                .HasMany(e => e.EgitilenKonferans)
                .WithMany(k => k.Egitmenler)
                .UsingEntity<Dictionary<string, object>>(
                    "EgitmenKonferans",
                    j => j.HasOne<Konferans>().WithMany().HasForeignKey("KonferansId"),
                    j => j.HasOne<Egitmen>().WithMany().HasForeignKey("EgitmenId"));

            // Configure the many-to-many relationship between Konferans and Uye
            modelBuilder.Entity<Konferans>()
                .HasMany(k => k.Katilimcilar)
                .WithMany(u => u.katilinanKonferanslar)
                .UsingEntity<Dictionary<string, object>>(
                    "KonferansUye",
                    j => j.HasOne<Uye>().WithMany().HasForeignKey("UyeId"),
                    j => j.HasOne<Konferans>().WithMany().HasForeignKey("KonferansId"));

            // Configure the one-to-many relationship between Konferans to Tartisma
            modelBuilder.Entity<Konferans>()
                .HasMany(k => k.Tartismalar)
                .WithOne(t => t.Konferans)
                .HasForeignKey(e => e.Id)
                .IsRequired(false);

            modelBuilder.Entity<Konferans>()
                .HasMany(k => k.Paylasimlar)
                .WithOne(p => p.PaylasilanKonferans)
                .HasForeignKey(e => e.Id)
                .IsRequired(false);
        }

        public DbSet<Konferans> Konferanslar { get; set; }
        public DbSet<Duyurular> Duyurular { get; set; }
        public DbSet<Uye> Uyeler { get; set; }
        public DbSet<Egitmen> Egitmenler { get; set; }
        public DbSet<Tartisma> Tartisma { get; set; }
        public DbSet<Paylasim> Paylasim { get; set; }
    }
}
