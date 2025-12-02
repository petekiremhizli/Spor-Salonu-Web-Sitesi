using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenterProject.Models
{
    public class AntrenorHizmet
    {
        [ForeignKey("Antrenor")]
        public int AntrenorId { get; set; }
        public Antrenor Antrenor { get; set; }


        [ForeignKey("Hizmet")]
        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }
    }


}
