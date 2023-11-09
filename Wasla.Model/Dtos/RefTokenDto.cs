using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class RefTokenDto
    {
        [Required(ErrorMessage ="refTokenReuired")]
        public string RefToken { get; set; }
    }
}
