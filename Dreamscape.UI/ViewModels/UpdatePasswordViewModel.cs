using Dreamscape.Application.Users;
using System.ComponentModel.DataAnnotations;

namespace Dreamscape.UI.ViewModels
{
    public class UpdatePasswordViewModel
    {
        [Required]
        public UserViewModel User;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation new password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
