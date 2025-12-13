using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Entities
{
    public class Antrenor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Ad { get; set; }

        [Required]
        public string Soyad { get; set; }

        public string UzmanlikAlani { get; set; } 


        
        public int BaslangicSaati { get; set; } 
        public int BitisSaati { get; set; }     

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<Randevu> Randevular { get; set; }
    }
}