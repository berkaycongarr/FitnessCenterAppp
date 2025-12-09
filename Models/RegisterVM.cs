using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string Username { get; set; } // Ad Soyad olarak kullanacağız

        [Required(ErrorMessage = "Email zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone]
        public string Phone { get; set; }

        public string Gender { get; set; } // Erkek/Kadın

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [MinLength(3, ErrorMessage = "Şifre en az 3 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        public string RePassword { get; set; }
    }
}