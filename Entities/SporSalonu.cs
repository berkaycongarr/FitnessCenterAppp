using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Entities
{
    public class SporSalonu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Salon Adı")]
        public string Ad { get; set; }

        [Display(Name = "Adres")]
        public string? Adres { get; set; }

        [Display(Name = "Şehir")]
        public string? Sehir { get; set; }

        public string CalismaSaatleri { get; set; } // "07:00-23:00"
    }
}