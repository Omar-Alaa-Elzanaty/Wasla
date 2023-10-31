﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class RiderRegisterDto
    {
        [Required(ErrorMessage = "phoneNumberRequired")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(20,ErrorMessage = "PasswordLength", MinimumLength= 6)]
        public string Password { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage ="firstNameRequired")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "lastNameRequired")]
        public string? LastName { get; set; }
       
        [Required(ErrorMessage = "userNameRequired")]
        public string UserName { get; set; }
        [JsonIgnore]
        public string Role { get; set; } = "Rider";
    }
}
