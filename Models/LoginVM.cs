using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Kullanıcı adı veya Email zorunludur.")]
        [Display(Name = "Kullanıcı Adı / Email")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
    }
}