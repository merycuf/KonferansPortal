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

            modelBuilder.Entity<Egitmen>()
                .HasKey(e => e.EgitmenId);

            // Configure the relationship between Egitmen and Uye
            modelBuilder.Entity<Egitmen>()
                .HasOne(e => e.UyeModel)
                .WithMany() // No navigation property in Uye
                .HasForeignKey(e => e.UyeId);

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
                .HasForeignKey("TartismalarId")
                .IsRequired(false);

            modelBuilder.Entity<Tartisma>()
                .HasOne(k => k.Konferans)
                .WithMany(t => t.Tartismalar)
                .HasForeignKey("KonferansId")
                .IsRequired(false);

            modelBuilder.Entity<OnKayit>()
                .HasOne(k => k.uye)
                .WithMany(t => t.onKayitKonferanslar)
                .HasForeignKey("UyeId")
                .IsRequired(true);

            modelBuilder.Entity<OnKayit>()
                .HasOne(k => k.konferans)
                .WithMany(t => t.OnKayitListe)
                .HasForeignKey("KonferansId")
                .IsRequired(true);

            modelBuilder.Entity<Tartisma>()
                .Property(f => f.Id).UseIdentityColumn(seed: 1, increment:1);

            modelBuilder.Entity<Konferans>()
                .HasMany(k => k.Paylasimlar)
                .WithOne(p => p.PaylasilanKonferans)
                .HasForeignKey("PaylasimId")
                .IsRequired(false);

            modelBuilder.Entity<Paylasim>()
                .HasOne(p => p.PaylasilanKonferans)
                .WithMany(k => k.Paylasimlar)
                .HasForeignKey("KonferansId")
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
