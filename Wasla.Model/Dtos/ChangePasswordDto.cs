using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "tokenRequired")]
        public string token { get; set; }
        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(20, ErrorMessage = "PasswordLength", MinimumLength = 6)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(20, ErrorMessage = "PasswordLength", MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
}
