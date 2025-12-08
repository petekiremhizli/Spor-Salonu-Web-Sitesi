using System.ComponentModel.DataAnnotations;

namespace FitnessCenterProject.Models
{
    public class SignUpViewModel
    {
        [Required]
        public string Ad { get; set; }

        [Required]
        public string Soyad { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }

        public string Telefon { get; set; }
    }
}
