using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterProject.Models
{

    // DbContext'imizi IdentityDbContext'ten türetiyoruz
    // Uye sınıfımız IdentityUser'dan türetildiği için buraya Uye yazıyoruz
    public class FitnessContext : IdentityDbContext<Uye>
    {
        // Constructor: DbContextOptions alır, base sınıfa iletir
        public FitnessContext(DbContextOptions<FitnessContext> options) : base(options)
        {
        }


        // DbSet'ler: Her tablo için bir DbSet tanımlıyoruz
        public DbSet<Antrenor> Antrenorler { get; set; }          // Antrenörler tablosu
        public DbSet<Hizmet> Hizmetler { get; set; }              // Hizmetler tablosu
        public DbSet<Randevu> Randevular { get; set; }            // Randevular tablosu
        public DbSet<Musaitlik> Musaitlikler { get; set; }        // Antrenör müsaitlik saatleri
        public DbSet<AntrenorHizmet> AntrenorHizmetler { get; set; } // Many-to-Many junction table
        // OnModelCreating: Model ilişkilerini ve FK'leri burada tanımlıyoruz
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Identity tablolarının doğru çalışması için base çağrısı
            base.OnModelCreating(modelBuilder);

            // ================================
            // Antrenor-Hizmet Many-to-Many ilişkisi
            // Composite key olarak AntrenorId ve HizmetId belirlenir
            modelBuilder.Entity<AntrenorHizmet>()
                .HasKey(ah => new { ah.AntrenorId, ah.HizmetId });

            // Antrenor tarafı
            modelBuilder.Entity<AntrenorHizmet>()
                .HasOne(ah => ah.Antrenor)             // Antrenor ile ilişki
                .WithMany(a => a.AntrenorHizmetler)   // Antrenor'de koleksiyon
                .HasForeignKey(ah => ah.AntrenorId);  // FK

            // Hizmet tarafı
            modelBuilder.Entity<AntrenorHizmet>()
                .HasOne(ah => ah.Hizmet)               // Hizmet ile ilişki
                .WithMany(h => h.AntrenorHizmetler)   // Hizmet'te koleksiyon
                .HasForeignKey(ah => ah.HizmetId);    // FK

            // ================================
            // Musaitlik One-to-Many (Antrenor -> Musaitlik)
            modelBuilder.Entity<Musaitlik>()
                .HasOne(m => m.Antrenor)               // Musaitlik bir Antrenora ait
                .WithMany(a => a.Musaitlikler)        // Antrenor koleksiyonu
                .HasForeignKey(m => m.AntrenorId);    // FK

            // ================================
            // Randevu ilişkileri
            // Randevu -> Uye
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Uye)                    // Randevu bir üyeye ait
                .WithMany(u => u.Randevular)          // Üye koleksiyonu
                .HasForeignKey(r => r.UyeId);         // FK

            // Randevu -> Antrenor
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Antrenor)              // Randevu bir antrenöre ait
                .WithMany(a => a.Randevular)          // Antrenör koleksiyonu
                .HasForeignKey(r => r.AntrenorId);    // FK

            // Randevu -> Hizmet
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Hizmet)                // Randevu bir hizmete ait
                .WithMany(h => h.Randevular)          // Hizmet koleksiyonu
                .HasForeignKey(r => r.HizmetId);      // FK
        }
    }



}
