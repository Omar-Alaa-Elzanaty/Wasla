using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class AcceptFollowRequestCommand
    {
        public string SenderId { get; set; }
        public string FolowerId { get; set; }
    }
}
