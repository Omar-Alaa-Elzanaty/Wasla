using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;

namespace Wasla.Services.HlepServices.MediaSerivces
{
    public interface IMediaSerivce
    {
        Task<string> AddAsync(IFormFile media);
        Task<string> AddAsync(MediaFile media);
        Task<string> UpdateAsync(string oldUrl, MediaFile newMedia);
        Task DeleteAsync(string url);
        Task<string> UpdateAsync(string oldUrl, IFormFile newMedia);

    }
}
