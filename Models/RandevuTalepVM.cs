using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class RandevuTalepVM
    {
        [Required(ErrorMessage = "Lütfen bir hizmet seçiniz.")]
        [Display(Name = "Hizmet")]
        public int HizmetId { get; set; }

        [Required(ErrorMessage = "Lütfen bir eğitmen seçiniz.")]
        [Display(Name = "Eğitmen")]
        public int AntrenorId { get; set; }

        [Required(ErrorMessage = "Tarih seçiniz.")]
        [DataType(DataType.Date)]
        public DateTime Tarih { get; set; }

        [Required(ErrorMessage = "Saat seçiniz.")]
        public string Saat { get; set; } // Örn: "09:00", "14:00"
    }
}