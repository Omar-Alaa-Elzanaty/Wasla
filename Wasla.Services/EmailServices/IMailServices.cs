using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Services.EmailServices
{
	public interface IMailServices
	{
		Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile>? attachments = null);
	}
}
