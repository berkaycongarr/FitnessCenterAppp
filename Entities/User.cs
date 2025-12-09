using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(50)]
        public string Username { get; set; } // Ad Soyad

        [Required]
        public string Password { get; set; } // Şifre

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? Phone { get; set; }
        public string? Gender { get; set; }

        // Roller: "admin", "uye", "antrenor"
        public string Role { get; set; } = "uye";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // İlişkiler
        public Antrenor? Antrenor { get; set; }
        public ICollection<Randevu> Randevular { get; set; }
    }
}