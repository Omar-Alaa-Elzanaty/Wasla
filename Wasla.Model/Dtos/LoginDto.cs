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
        public string Password { get; set; }
        public string role { get; set; }
    }
}
