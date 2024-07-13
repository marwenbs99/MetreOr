using MetreOr.Data;
using MetreOr.Models;
using System.Text.RegularExpressions;

namespace MetreOr.ViewModels
{
    public class SignUpViewModel : AppUser
    {
        public string RepeatPassword { get; set; } = string.Empty;

        private bool IsValidEmail()
        {
            if (string.IsNullOrWhiteSpace(this.Email))
                return false;
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(this.Email);
        }

        private bool IsValidePhoneNumber()
        {
            if (!(this.PhoneNumber.Length == 8) || !int.TryParse(this.PhoneNumber, out _))
            {
                return false;
            }
            return true;
        }

        private bool IsUserYoung()
        {
            if (DateTime.Now.Year - this.Birthday.Year < 18)
            {
                return true;
            }
            return false;
        }

        private bool IsPasswordGood()
        {
            if (string.IsNullOrWhiteSpace(this.Password))
            {
                return false;
            }

            // Vérifie si le mot de passe contient au moins 8 caractères
            if (this.Password.Length < 8)
            {
                return false;
            }

            // Vérifie si le mot de passe contient au moins une majuscule
            bool hasUpperCase = Regex.IsMatch(this.Password, @"[A-Z]");

            // Vérifie si le mot de passe contient au moins un chiffre
            bool hasDigit = Regex.IsMatch(this.Password, @"\d");

            return hasUpperCase && hasDigit;
        }

        public Dictionary<bool, string> VerifyForm()
        {
            if (this.IsUserYoung())
            {
                return new Dictionary<bool, string>
                {
                    { false, "Vous devez avoir au moins 18 ans pour créer un compte. Veuillez réessayer." }
                };
            }

            if (!this.IsValidEmail())
            {
                return new Dictionary<bool, string>
                {
                    { false, "Le format de l email entré n est pas valide. Veuillez réessayer." }
                };
            }

            if (!this.IsValidePhoneNumber())
            {
                return new Dictionary<bool, string>
                {
                    { false, "Le format du numéro de téléphone entré n est pas valide. Veuillez réessayer.!" }
                };
            }

            if (!this.IsPasswordGood())
            {
                return new Dictionary<bool, string>
                {
                    { false, "Le mot de passe doit contenir au moins 8 caractères, une majuscule et un chiffre." }
                };
            }

            if (this.Password != this.RepeatPassword)
            {
                return new Dictionary<bool, string>
                {
                    { false, "Assurez-vous de remplir correctement le formulaire!" }
                };
            }
                return new Dictionary<bool, string>
            {
                { true, string.Empty }
            };
        }

    }
}
