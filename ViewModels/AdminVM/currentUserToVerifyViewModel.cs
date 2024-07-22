using MetreOr.Enum;
using System.ComponentModel.DataAnnotations;

namespace MetreOr.ViewModels.AdminVM
{
    public class CurrentUserToVerifyViewModel
    {
        public Guid UserGuid { get; set; }
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public string Adresse { get; set; } = string.Empty;
        public BtnActionType ActionType { get; set; }
        public IList<CheckBoxItem> CheckBoxes { get; set; } = new List<CheckBoxItem>();
    }
    public class CheckBoxItem
    {
        public string Key { get; set; }
        public bool Value { get; set; }
    }
}
