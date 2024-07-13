using MetreOr.Models;

namespace MetreOr.ViewModels
{
    public class SignUpViewModel: AppUser
    {
        public string RepeatPassword { get; set; }= string.Empty;
    }
}
