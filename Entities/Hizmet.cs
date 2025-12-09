using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Entities
{
    public class Hizmet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Ad { get; set; } // Özel Ders, Grup Dersi vb.

        [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "decimal(18,2)")]
        public decimal Ucret { get; set; }

        public int Sure { get; set; } // Dakika cinsinden (Örn: 60)

        // Bu hizmeti alan randevular
        public ICollection<Randevu> Randevular { get; set; }
    }
}