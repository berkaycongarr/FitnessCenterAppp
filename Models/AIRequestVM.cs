using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class AIRequestVM
    {
        [Required(ErrorMessage = "Boy bilgisi zorunludur.")]
        [Range(100, 250, ErrorMessage = "Lütfen geçerli bir boy giriniz (cm).")]
        public int Boy { get; set; } 

        [Required(ErrorMessage = "Kilo bilgisi zorunludur.")]
        [Range(30, 200, ErrorMessage = "Lütfen geçerli bir kilo giriniz (kg).")]
        public double Kilo { get; set; }

        [Required]
        public string Cinsiyet { get; set; }

        [Required]
        public string Hedef { get; set; } 

       
        public string? AIResult { get; set; }
    }
}