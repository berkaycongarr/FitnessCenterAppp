using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Entities
{
    public class SporSalonu
    {
        [Key]
        public int Id { get; set; }
        public string Ad { get; set; }
        public string? Adres { get; set; }
        public string CalismaSaatleri { get; set; }
    }
}