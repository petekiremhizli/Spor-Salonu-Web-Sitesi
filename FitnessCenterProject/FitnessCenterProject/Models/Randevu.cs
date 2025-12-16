using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenterProject.Models
{
    public class Randevu
    {
        public int Id { get; set; }


        [ForeignKey("Uye")]
        public string UyeId { get; set; }
        public Uye Uye { get; set; }


        [ForeignKey("Antrenor")]

        public int AntrenorId { get; set; }
        public Antrenor Antrenor { get; set; }


        [ForeignKey("Hizmet")]

        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }

        [Required(ErrorMessage = "Başlangıç zamanı zorunludur.")]
        public DateTime BaslangicZamani { get; set; }



        [Required(ErrorMessage = "Bitiş zamanı zorunludur.")]
        public DateTime BitisZamani { get; set; }  // BaslangicZamani + Hizmet.Sure


        [Required(ErrorMessage = "Durum zorunludur.")]
        [StringLength(20)]
        public string Durum { get; set; }         // Beklemede, Onaylandi, Iptal
    }




}
