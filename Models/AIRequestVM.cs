using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class AIRequestVM
    {
        [Required(ErrorMessage = "Boy bilgisi zorunludur.")]
        [Range(100, 250, ErrorMessage = "Lütfen geçerli bir boy giriniz (cm).")]
        public int Boy { get; set; } // cm cinsinden (Örn: 180)

        [Required(ErrorMessage = "Kilo bilgisi zorunludur.")]
        [Range(30, 200, ErrorMessage = "Lütfen geçerli bir kilo giriniz (kg).")]
        public double Kilo { get; set; }

        [Required]
        public string Cinsiyet { get; set; } // "Kadın", "Erkek"

        [Required]
        public string Hedef { get; set; } // "Kilo Verme", "Kas Yapma", "Form Koruma"

        // Yapay Zekanın ürettiği cevabı burada taşıyacağız
        public string? AIResult { get; set; }
    }
}