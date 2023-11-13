using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class ConfirmEmailDto
    {
        [Required(ErrorMessage = "otpReuired")]
        public string RecOtp { get; set; }
        [Required(ErrorMessage = "EmailRequired")]
        public string Email { get; set; }
    }
}
