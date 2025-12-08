using System.ComponentModel.DataAnnotations;

namespace FitnessCenterProject.Models
{
    public class Antrenor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad soyad alanı zorunludur.")]
        [StringLength(50)]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur.")]
        [StringLength(100)]
        public string Uzmanlik { get; set; }



        // Antrenörün verebileceği hizmetler (Many-to-Many)
        public ICollection<AntrenorHizmet>? AntrenorHizmetler { get; set; }

        // Antrenörün randevuları
        public ICollection<Randevu>? Randevular { get; set; }

        // Müsaitlik saatleri
        public ICollection<Musaitlik>? Musaitlikler { get; set; }


    }

}