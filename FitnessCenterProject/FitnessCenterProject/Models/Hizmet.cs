using System.ComponentModel.DataAnnotations;

namespace FitnessCenterProject.Models
{
    public class Hizmet
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [StringLength(50)]
        public string Ad { get; set; }     


        [Required(ErrorMessage = "Süre zorunludur.")]
        [Range(1, 600, ErrorMessage = "Süre 1 ile 600 dakika arasında olmalıdır.")]
        public int Sure { get; set; }           


        [Required(ErrorMessage = "Ücret zorunludur.")]
        [Range(0, 10000, ErrorMessage = "Ücret 0 ile 1000 arasında olmalıdır.")]
        public decimal Ucret { get; set; }       
        public ICollection<AntrenorHizmet>? AntrenorHizmetler { get; set; }
        public ICollection<Randevu>? Randevular { get; set; }
    }


}
