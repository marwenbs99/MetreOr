using System.ComponentModel.DataAnnotations;

namespace MetreOr.Models
{
    public class Immobilier
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Adresse { get; set; } = string.Empty;
        [Required]
        public int Surface { get; set; } = 0;
        [Required]
        public decimal Prix { get; set; } = 0;
        [Required]
        public int RevenuLocatif { get; set; } = 0;
    }
}
