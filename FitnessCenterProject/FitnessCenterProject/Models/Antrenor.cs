    using System.ComponentModel.DataAnnotations;

    namespace FitnessCenterProject.Models
    {
        public class Antrenor
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Ad soyad alanı zorunludur.")]
            [StringLength(50)]
            public string AdSoyad { get; set; }

            public ICollection<AntrenorHizmet>? AntrenorHizmetler { get; set; }
            public ICollection<Randevu>? Randevular { get; set; }
            public ICollection<Musaitlik>? Musaitlikler { get; set; }


        }

    }