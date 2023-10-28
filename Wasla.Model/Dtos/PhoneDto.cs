using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class PhoneDto
    {
        [Required(ErrorMessage = "phoneNumberRequired")]
        public string Phone { get; set; }
    }
}
