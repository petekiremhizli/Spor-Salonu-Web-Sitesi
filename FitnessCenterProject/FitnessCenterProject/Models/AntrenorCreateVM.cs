using FitnessCenterProject.Models;
using System.ComponentModel.DataAnnotations;

public class AntrenorCreateVM
{
    [Required(ErrorMessage = "Antrenör adı zorunludur")]
    public string AdSoyad { get; set; }

    [Required(ErrorMessage = "En az bir uzmanlık alanı seçmelisiniz")]
    public List<int> SeciliHizmetler { get; set; } = new();

    public List<Hizmet> Hizmetler { get; set; }
}
