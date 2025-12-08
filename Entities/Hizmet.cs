using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Entities
{
    public class Hizmet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Hizmet Adı")]
        public string Ad { get; set; } // Örn: Özel Ders, Grup Pilates

        [Display(Name = "Ücret")]
        public decimal Ucret { get; set; }

        [Display(Name = "Süre (Dakika)")]
        public int Sure { get; set; } // Örn: 45, 60, 90 dakika

        // Bu hizmet hangi randevularda var
        public ICollection<Randevu> Randevular { get; set; }
    }
}