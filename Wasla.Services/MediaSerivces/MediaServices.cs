using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;
using Wasla.Model.Helpers;
using Wasla.Services.Exceptions;

namespace Wasla.Services.MediaSerivces
{
    public class MediaServices:IMediaSerivces
    {
        private readonly IWebHostEnvironment _host;
		private readonly StringBuilder _defaultPath;
		private readonly string _fileName;
		private readonly IStringLocalizer<IMediaSerivces> _localization;

		bool ImageConstrains(IFormFile extension)
		{
			return true;
		}
		bool VideoConstrains(IFormFile extension)
		{
			return true;
		}
		bool IsImageExtension(string ext) => (ext == ".PNG" || ext == ".jpg");
		bool IsVideoExtension(string ext) => (ext == "d" || ext == "dd");

		public MediaServices(
			IWebHostEnvironment host,
			IHttpContextAccessor contextAccessor,
			IStringLocalizer<IMediaSerivces> localization)
		{
			_host = host;
			_defaultPath = new StringBuilder(@$"{contextAccessor.HttpContext?.Request.Scheme}://{contextAccessor?.HttpContext?.Request.Host}/FOLDER/");
			_fileName = Guid.NewGuid().ToString();
			_localization = localization;
		}
		public async Task<string> AddAsync(IFormFile media)
        {
			string RootPath = _host.WebRootPath;
			string Extension = Path.GetExtension(media.FileName);
			string MediaFolderPath = "";
			string path = "";
			if (IsImageExtension(Extension))
			{
				MediaFolderPath = Path.Combine(RootPath, "Images");
				path += _defaultPath.Replace("FOLDER","Images");
			}
			else if (IsVideoExtension(Extension))
			{
				MediaFolderPath = Path.Combine(RootPath, "Videos");
				path += _defaultPath.Replace("FOLDER", "Videos");
			}
			else
			{
				throw new BadRequestException(_localization["UploadMediaFail"].Value);
			}
			using (FileStream fileStreams = new(Path.Combine(MediaFolderPath,
											_fileName + Extension), FileMode.Create))
			{
				media.CopyTo(fileStreams);
			}
			return path + _fileName + Extension;
		}
        public async Task<bool> RemoveAsync(string url)
		{
			try
			{
				string RootPath = _host.WebRootPath.Replace("\\\\", "\\");
				var mediaNameToDelete = Path.GetFileNameWithoutExtension(url);
				var EXT = Path.GetExtension(url);
				string? oldPath = "";
				if (IsVideoExtension(EXT))
					oldPath = $@"{RootPath}\Videos\{mediaNameToDelete}{EXT}";
				else if (IsImageExtension(EXT))
					oldPath = $@"{RootPath}\Images\{mediaNameToDelete}{EXT}";
				else
				{
					throw new BadRequestException(_localization["DeleteMediaFail"].Value);
				}
				if (File.Exists(oldPath))
				{
					File.Delete(oldPath);
					return true;
				}
				throw new BadRequestException(_localization["NotFoundMedia"].Value);
			}
			catch
			{
				throw new BadRequestException(_localization["DeleteMediaFail"].Value);
			}
		}
        public async Task<string> UpdateAsync(string oldUrl, IFormFile newMedia)
        {
			ServicesResponse<string> response = new ServicesResponse<string>();
			string? newMediaUrl=null;
			string Extension = Path.GetExtension(newMedia.FileName);
			if (ImageConstrains(newMedia))
			{ 
				newMediaUrl =_defaultPath.Replace("FOLDER","Image").ToString();
			}
			else if (VideoConstrains(newMedia))
			{
				newMediaUrl = _defaultPath.Replace("FOLDER", "Records").ToString();
			}

			if(newMediaUrl is null)
			{
				throw new BadRequestException(_localization["NotFoundMedia"].Value);
			}
			newMediaUrl += _fileName + Extension;
			if (oldUrl == newMediaUrl)
			{
				return oldUrl;
			}
			if (!await RemoveAsync(oldUrl)) 
			{
				throw new BadRequestException(_localization["UploadMediaFail"].Value);
			}
			var addResult = await AddAsync(newMedia);
			if (addResult == null)
			{
				throw new BadRequestException(_localization["UploadMediaFail"].Value);
			}
			return addResult;
		}
	}
}
