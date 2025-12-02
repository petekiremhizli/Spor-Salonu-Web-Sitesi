using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessCenterProject.Models
{
    public class Musaitlik
    {
        public int Id { get; set; }

        [Required]
        public DayOfWeek Gun { get; set; }

        [Required]
        public TimeSpan BaslangicSaati { get; set; }

        [Required]
        public TimeSpan BitisSaati { get; set; }


        [ForeignKey("Antrenor")]
        public int AntrenorId { get; set; }
        public Antrenor Antrenor { get; set; }
    }




}
