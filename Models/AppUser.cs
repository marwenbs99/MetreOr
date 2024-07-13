using System.ComponentModel.DataAnnotations;

namespace MetreOr.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string Adresse { get; set; } = string.Empty;
        [Required]
        public required string Password { get; set; }    
        public DateTime DateOfInscription { get; set; } = DateTime.Now;
        public bool IsVerified  { get; set; } = false;
    }
}
