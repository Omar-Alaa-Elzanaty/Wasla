using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;

namespace Wasla.Services.MediaSerivces
{
	public interface IMediaSerivce
	{
		Task<string> AddAsync(IFormFile media);
		Task RemoveAsync(string url);
		Task<string> UpdateAsync(string oldUrl, IFormFile newMedia);

	}
}
