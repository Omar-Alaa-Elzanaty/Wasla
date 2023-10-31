using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Services.Exceptions
{
    public class KeynotFoundException : Exception
    {
        public KeynotFoundException(string message) : base(message)
        {
        }
    }
}
