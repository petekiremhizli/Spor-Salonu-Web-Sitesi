using System.ComponentModel.DataAnnotations;

namespace FitnessCenterProject.Models.Api
{
    public class HizmetApiModel
    {
        public int Id { get; set; }

        public string Ad { get; set; }

        public int Sure { get; set; }

        public decimal Ucret { get; set; }
    }
}
