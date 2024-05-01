using System.ComponentModel.DataAnnotations;

namespace Wasla.Model.Helpers
{
    public class RegisterHelp
    {
        //  [Required(ErrorMessage = "phoneNumberRequired")]
        public string? PhoneNumber { get; set; }

        [StringLength(100,MinimumLength =6)]
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
