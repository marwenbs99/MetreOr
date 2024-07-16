using MetreOr.Models;

namespace MetreOr.ViewModels.AdminVM
{
    public class UserToVerifyViewModel
    {
        public IEnumerable<AppUser> UserToVerify { get; set; } = new List<AppUser>();
    }
}
