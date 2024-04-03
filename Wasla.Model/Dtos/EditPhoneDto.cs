using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class EditPhoneDto
    {
        [Required(ErrorMessage = "tokenRequired")]
        public string Reftoken { get; set; }
        [Required(ErrorMessage = "phoneNumberRequired")]
        public string Phone { get; set; }
    }
}
