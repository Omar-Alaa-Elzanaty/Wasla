﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers
{
    public class RegisterHelp
    {
      //  [Required(ErrorMessage = "phoneNumberRequired")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "PasswordRequired")]
        public string Password { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage = "firstNameRequired")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "lastNameRequired")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "userNameRequired")]
        public string UserName { get; set; }
    }
}
