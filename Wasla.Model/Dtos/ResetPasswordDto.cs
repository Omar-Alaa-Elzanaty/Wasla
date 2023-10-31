using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "phoneNumberRequired")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(20, ErrorMessage = "PasswordLength", MinimumLength = 6)]
        public string newPassword { get; set; }
    }
}
