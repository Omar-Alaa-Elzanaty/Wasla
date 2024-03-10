using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Services.Exceptions
{
    public class NotImplementeException:Exception
    {
      public  NotImplementeException(string message) : base(message) { }
    }
}
