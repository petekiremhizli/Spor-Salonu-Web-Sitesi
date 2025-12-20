using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FitnessCenterProject.Models
{
    public class DiyetInputModel
    {
        [Required(ErrorMessage = "Yaş zorunludur.")]
        [Range(1, 120, ErrorMessage = "Geçerli bir yaş giriniz.")]
        public int Yas { get; set; }

        [Required(ErrorMessage = "Boy zorunludur.")]
        [Range(50, 250, ErrorMessage = "Geçerli bir boy giriniz.")]
        public int Boy { get; set; } // cm

        [Required(ErrorMessage = "Kilo zorunludur.")]
        [Range(10, 300, ErrorMessage = "Geçerli bir kilo giriniz.")]
        public int Kilo { get; set; } // kg

        [Required(ErrorMessage = "Hedef seçiniz.")]
        public string Hedef { get; set; } // "Kilo Alma", "Kilo Verme", "Sabit Kalma"
    }
}
