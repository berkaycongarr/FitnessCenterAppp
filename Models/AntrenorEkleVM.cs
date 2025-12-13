using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class AntrenorEkleVM
    {
        [Required(ErrorMessage = "Ad zorunludur")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon zorunludur")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur")]
        public string UzmanlikAlani { get; set; } 

        [Required(ErrorMessage = "Şifre zorunludur")]
        public string Password { get; set; }

        [Required]
        [Range(0, 23)]
        public int BaslangicSaati { get; set; }

        [Required]
        [Range(0, 23)]
        public int BitisSaati { get; set; }


    }
}