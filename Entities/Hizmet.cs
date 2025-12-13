using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Entities
{
    public class Hizmet
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        public string Ad { get; set; }

        [Column(TypeName = "decimal(18,2)")] 
        public decimal Ucret { get; set; }

        public int Sure { get; set; } 

      
        public virtual ICollection<Randevu>? Randevular { get; set; }
    }
} 