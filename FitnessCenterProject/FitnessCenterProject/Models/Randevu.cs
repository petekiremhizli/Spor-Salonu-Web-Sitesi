using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterProject.Models
{
    public class Randevu
    {
        public int Id { get; set; }

        // Formdan gelecek alanlar
        [Required(ErrorMessage = "Hizmet seçimi zorunludur.")]
        public int HizmetId { get; set; }

        [Required(ErrorMessage = "Antrenör seçimi zorunludur.")]
        public int AntrenorId { get; set; }

        [Required(ErrorMessage = "Lütfen gün ve saat seçiniz.")]
        public DateTime BaslangicZamani { get; set; }

        public DateTime BitisZamani { get; set; }

        // Backend tarafından atanacak alanlar
        [BindNever]
        public string UyeId { get; set; } = string.Empty;

        [BindNever]
        public string Durum { get; set; } = string.Empty;

        // Navigation property'ler (nullable yaptık)
        [BindNever]
        public Uye? Uye { get; set; }

        [BindNever]
        public Antrenor? Antrenor { get; set; }

        [BindNever]
        public Hizmet? Hizmet { get; set; }
    }
}
