using FitnessCenterProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterProject.Data
{
    public class AppDbContext : IdentityDbContext<Uye>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Antrenor> Antrenorler { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<Musaitlik> Musaitlikler { get; set; }
        public DbSet<AntrenorHizmet> AntrenorHizmetler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Antrenor ↔ Hizmet (Many-to-Many) ara tablo
            modelBuilder.Entity<AntrenorHizmet>()
                .HasKey(ah => new { ah.AntrenorId, ah.HizmetId });

            modelBuilder.Entity<AntrenorHizmet>()
                .HasOne(ah => ah.Antrenor)
                .WithMany(a => a.AntrenorHizmetler)
                .HasForeignKey(ah => ah.AntrenorId);

            modelBuilder.Entity<AntrenorHizmet>()
                .HasOne(ah => ah.Hizmet)
                .WithMany(h => h.AntrenorHizmetler)
                .HasForeignKey(ah => ah.HizmetId);

            modelBuilder.Entity<Hizmet>()
            .Property(h => h.Ucret)
            .HasPrecision(10, 2);

            // Antrenor → Musaitlik (1-N)
            modelBuilder.Entity<Musaitlik>()
                .HasOne(m => m.Antrenor)
                .WithMany(a => a.Musaitlikler)
                .HasForeignKey(m => m.AntrenorId);

            // Antrenor → Randevu (1-N)
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Antrenor)
                .WithMany(a => a.Randevular)
                .HasForeignKey(r => r.AntrenorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Hizmet → Randevu (1-N)
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Hizmet)
                .WithMany(h => h.Randevular)
                .HasForeignKey(r => r.HizmetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Uye (Identity) → Randevu (1-N)
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Uye)
                .WithMany(u => u.Randevular)
                .HasForeignKey(r => r.UyeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
