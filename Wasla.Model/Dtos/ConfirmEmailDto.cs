using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class ConfirmEmailDto
    {
        [Required(ErrorMessage = "otpReuired")]
        public string RecOtp { get; set; }
        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }
    }
}
