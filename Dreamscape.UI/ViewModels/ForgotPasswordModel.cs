using System.ComponentModel.DataAnnotations;

namespace Dreamscape.UI.ViewModels
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
