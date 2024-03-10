

using System.ComponentModel.DataAnnotations;

namespace Wasla.Model.Dtos
{
    public class ResetPasswordByEmailDto
    {
        [Required(ErrorMessage = "EmailRequired")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(20, ErrorMessage = "PasswordLength", MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
}
