using System.ComponentModel.DataAnnotations;
using MetreOr.Extensions;

namespace MetreOr.ViewModels
{
    public class IdentityVerificationViewModel
    {
        //[Required(ErrorMessage = "Veuillez télécharger une image du recto de votre carte d'identité.")]
        [Display(Name = "Carte d'identité recto")]
        public IFormFile CIDRecto { get; set; }

        //[Required(ErrorMessage = "Veuillez télécharger une image du verso de votre carte d'identité.")]
        [Display(Name = "Carte d'identité verso")]
        public IFormFile CIDVerso { get; set; }


        public required string FirstName { get; set; }
        
        public required string LastName { get; set; }

        public required string Email { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime Birthday { get; set; }

        public string Adresse { get; set; } = string.Empty;

        public required string Password { get; set; }

        public bool AreFilesValid()
        {
            var validator = new FileValidationHelper();
            return validator.IsImage(CIDRecto) && validator.IsImage(CIDVerso);
        }
    }
}
