using System.ComponentModel.DataAnnotations;

namespace FitnessCenterProject.Models // Kendi projenizin Namespace'ini kullanın!
{
    public class YapayZekaEntegrasyonu
    {
        // Vücut Tipi / Hedef seçimi için
        [Required(ErrorMessage = "Lütfen hedefinizi seçin.")]
        [Display(Name = "Hedef / Vücut Tipi")]
        public string BodyType { get; set; }

        // Boy (cm) girişi için
        [Required(ErrorMessage = "Boy bilgisi zorunludur.")]
        [Range(50, 250, ErrorMessage = "Boyunuz 50 ile 250 cm arasında olmalıdır.")]
        [Display(Name = "Boy (cm)")]
        public double Height { get; set; }

        // Kilo (kg) girişi için
        [Required(ErrorMessage = "Kilo bilgisi zorunludur.")]
        [Range(30, 300, ErrorMessage = "Kilonuz 30 ile 300 kg arasında olmalıdır.")]
        [Display(Name = "Kilo (kg)")]
        public double Weight { get; set; }

        // AI'dan gelecek sonuç bu alana yazılacak
        public string AIResponse { get; set; }
    }
}