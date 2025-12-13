using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Entities
{
    public class Randevu
    {
        [Key]
        public int Id { get; set; }
        public DateTime TarihSaat { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int AntrenorId { get; set; }
        [ForeignKey("AntrenorId")]
        public Antrenor Antrenor { get; set; }

        public int HizmetId { get; set; }
        [ForeignKey("HizmetId")]
        public Hizmet Hizmet { get; set; }

        
        public int Durum { get; set; } = 0;
    }
}