using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Entities
{
    public class Hizmet
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        public string Ad { get; set; } // Özel Ders, Grup Dersi vb.

        [Column(TypeName = "decimal(18,2)")] // Para formatı ayarı
        public decimal Ucret { get; set; }

        public int Sure { get; set; } // Dakika cinsinden (Örn: 60)

        // DÜZELTME BURADA:
        // 'ICollection<Randevu>' yanına '?' koyduk.
        // Artık "The Randevular field is required" hatası vermeyecek.
        public virtual ICollection<Randevu>? Randevular { get; set; }
    }
} 