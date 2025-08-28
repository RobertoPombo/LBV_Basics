using LBV_Basics.Models.Common;

namespace LBV_Basics.Models
{
    public class User : IBaseModel
    {
        public override string ToString() { return WindowsUserId + " | " + FirstName + " " + LastName; }

        public int Id { get; set; }
        public string WindowsUserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAdress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsAllowedToAnswer { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
    }
}
