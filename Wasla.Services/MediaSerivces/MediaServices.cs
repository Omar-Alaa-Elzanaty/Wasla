using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;
using Wasla.Model.Helpers;

namespace Wasla.Services.MediaSerivces
{
    internal class MediaServices
    {
        private readonly IWebHostEnvironment _host;
		private readonly StringBuilder _defaultPath;
		private readonly string _fileName;


		bool ImageConstrains(IFormFile extension)
		{
			return true;
		}
		bool VideoConstrains(IFormFile extension)
		{
			return true;
		}
		bool IsImageExtension(string ext) => (ext == "d" || ext == "dd");
		bool IsVideoExtension(string ext) => (ext == "d" || ext == "dd");

		public MediaServices(IWebHostEnvironment host, HttpContextAccessor contextAccessor)
		{
			_host = host;
			_defaultPath = new StringBuilder(@$"{contextAccessor.HttpContext?.Request.Scheme}://{contextAccessor?.HttpContext?.Request.Host}/FOLDER/");
			_fileName = Guid.NewGuid().ToString();
		}
		public async Task<ServicesResponse<string>> Add(IFormFile media)
        {
			ServicesResponse<string> response = new ServicesResponse<string>();
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
				response.Log = "invaild media extension";
				return response;
			}
			using (FileStream fileStreams = new(Path.Combine(MediaFolderPath,
											_fileName + Extension), FileMode.Create))
			{
				media.CopyTo(fileStreams);
			}
			response.IsSuccess=true;
			response.Entity = path + _fileName + Extension;
			return response;
		}
        public async Task<ServicesResponse<bool?>> Remove(string url)
		{
			ServicesResponse<bool?> response = new ServicesResponse<bool?>();
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
					response.Log = "Invalid Extension";
				}
				if (File.Exists(oldPath))
				{
					File.Delete(oldPath);
					response.IsSuccess = true;
					return response;
				}
				return response;
			}
			catch
			{
				return response;
			}
		}
        public async Task<ServicesResponse<string>> Update(string oldUrl, IFormFile newMedia)
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
				response.Log = "could not parse";
				return response;
			}
			newMediaUrl += _fileName + Extension;
			if (oldUrl == newMediaUrl)
			{
				response.IsSuccess = true;
				response.Entity = oldUrl;
				return response;
			}
			if (!Remove(oldUrl).Result.IsSuccess) 
			{
				response.Log = "couldn't update photo";
				return response;
			}
			var addResult = await Add(newMedia);
			if (addResult == null)
			{
				response.Log = "couldn't update Media file";
				return response;
			}
			response.IsSuccess = true;
			response.Entity = addResult.Entity;
			return response;
		}
	}
}
