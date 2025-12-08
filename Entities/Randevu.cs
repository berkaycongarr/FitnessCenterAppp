using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Entities
{
    public class Randevu
    {
        [Key]
        public int Id { get; set; }

        public DateTime TarihSaat { get; set; }

        // İlişkiler
        public int UserId { get; set; } // Randevuyu alan üye
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int AntrenorId { get; set; }
        [ForeignKey("AntrenorId")]
        public Antrenor Antrenor { get; set; }

        public int HizmetId { get; set; }
        [ForeignKey("HizmetId")]
        public Hizmet Hizmet { get; set; }

        // Onay Mekanizması İçin
        // 0: Bekliyor, 1: Onaylandı, 2: İptal
        public int Durum { get; set; } = 0;
    }
}