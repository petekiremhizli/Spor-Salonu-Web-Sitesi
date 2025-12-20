using FitnessCenterProject.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterProject.Models
{
    public class Musaitlik
    {
        public int Id { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        public TimeSpan BaslangicSaati { get; set; }

        [Required]
        public TimeSpan BitisSaati { get; set; }

        [Required]
        public int AntrenorId { get; set; }

        [ValidateNever]   // 🔥 ÇOK ÖNEMLİ
        public Antrenor Antrenor { get; set; }
    }
}