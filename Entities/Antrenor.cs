using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Entities
{
    public class Antrenor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Required]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        [Display(Name = "Uzmanlık Alanı")]
        public string UzmanlikAlani { get; set; } // Örn: Pilates, Bodybuilding, Yoga

        [Display(Name = "Çalışma Saatleri")]
        public string CalismaSaatleri { get; set; } // Örn: "09:00 - 18:00"

        // Login olabilmesi için User tablosuna bağlıyoruz
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        // Antrenörün dahil olduğu randevular
        public ICollection<Randevu> Randevular { get; set; }
    }
}