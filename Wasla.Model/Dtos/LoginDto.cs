using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wasla.Model.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "userNameRequired")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(20, ErrorMessage = "PasswordLength", MinimumLength = 6)]
        public string Password { get; set; }
        public string role { get; set; }
    }
}
