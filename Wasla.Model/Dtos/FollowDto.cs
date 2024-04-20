using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class FollowDto
    {
        public string SenderId { get; set; }
        public string FollowerId { get; set; }
    }
}
