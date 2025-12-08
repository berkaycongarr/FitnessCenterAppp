using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50)]
        public string Username { get; set; } // Genellikle Email veya Ad

        [StringLength(30)]
        public string? UserSurname { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email adresi zorunludur.")]
        [EmailAddress]
        public string Email { get; set; }

        public string? Phone { get; set; }

        public string? Gender { get; set; }

        // Role: "admin", "uye", "antrenor"
        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "uye";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // İlişkiler
        public Antrenor? Antrenor { get; set; } // Eğer kullanıcı bir antrenörse dolu olur
        public ICollection<Randevu> Randevular { get; set; } // Üyenin randevuları
    }
}