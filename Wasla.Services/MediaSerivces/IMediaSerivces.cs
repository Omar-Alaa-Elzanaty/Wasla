using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;

namespace Wasla.Services.MediaSerivces
{
	public interface IMediaSerivces
	{
		Task<ServicesResponse<string>> AddAsync(IFormFile media);
		Task<ServicesResponse<bool?>> RemoveAsync(string url);
		Task<ServicesResponse<string>> UpdateAsync(string oldUrl, IFormFile newMedia);

	}
}
