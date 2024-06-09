using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Localization;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Statics;
using Wasla.Services.Exceptions;

namespace Wasla.Services.HlepServices.MediaSerivces
{
    //TODO: Need to Upgrade
    public class MediaService : IMediaSerivce
    {
        private readonly IWebHostEnvironment _host;
        private readonly StringBuilder _defaultPath;
        private readonly IStringLocalizer<IMediaSerivce> _localization;

        bool ImageConstrains(IFormFile extension)
        {
            return true;
        }
        bool VideoConstrains(IFormFile extension)
        {
            return true;
        }
        //TODO: 
        bool IsImageExtension(string ext) { 
            
            ext = ext.ToLower();

            return true/*ext == ".png" || ext == ".jpg" || ext == ".jfif"*/; 
        }
        bool IsVideoExtension(string ext) => ext == "d" || ext == "dd";

        public MediaService(
            IWebHostEnvironment host,
            IHttpContextAccessor contextAccessor,
            IStringLocalizer<IMediaSerivce> localization)
        {
            _host = host;
            _defaultPath = new StringBuilder(@$"{contextAccessor.HttpContext?.Request.Scheme}://{contextAccessor?.HttpContext?.Request.Host}/FOLDER/");
            _localization = localization;
        }
        public async Task<string> AddAsync(IFormFile media)
        {
            if (media is null) return null;
            string RootPath = _host.WebRootPath;
            string file = Guid.NewGuid().ToString();
            string Extension = Path.GetExtension(media.FileName);
            StringBuilder mainPath = _defaultPath;
            string MediaFolderPath = "";
            string path = "";
            if (IsImageExtension(Extension))
            {
                MediaFolderPath = Path.Combine(RootPath, "Images");
                path += mainPath.Replace("FOLDER", "Images");
            }
            else if (IsVideoExtension(Extension))
            {
                MediaFolderPath = Path.Combine(RootPath, "Videos");
                path += mainPath.Replace("FOLDER", "Videos");
            }
            else
            {
                throw new BadRequestException(_localization["UploadMediaFail"].Value);
            }

            if(!Directory.Exists(MediaFolderPath))
            {
                Directory.CreateDirectory(MediaFolderPath);
            }

            using (Stream fileStreams = new FileStream(Path.Combine(MediaFolderPath, file + Extension), FileMode.Create))
            {
                media.CopyTo(fileStreams);
                fileStreams.Flush();
                fileStreams.Dispose();
            }

            return path + file + Extension;
        }
        public async Task<string>AddAsync(MediaFile media)
        {
            if (media is null) return null;
            string RootPath = _host.WebRootPath;
            string file = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(media.FileName);
            StringBuilder mainPath = _defaultPath;
            string MediaFolderPath = "";
            string path = "";
            if (IsImageExtension(extension))
            {
                MediaFolderPath = Path.Combine(RootPath, "Images");
                path += mainPath.Replace("FOLDER", "Images");
            }
            else if (IsVideoExtension(extension))
            {
                MediaFolderPath = Path.Combine(RootPath, "Videos");
                path += mainPath.Replace("FOLDER", "Videos");
            }
            else
            {
                throw new BadRequestException(_localization["UploadMediaFail"].Value);
            }

            if (!Directory.Exists(MediaFolderPath))
            {
                Directory.CreateDirectory(MediaFolderPath);
            }

            await File.WriteAllBytesAsync(path, Convert.FromBase64String(media.FileBase64));
            return path + file + extension;
        }
        public async Task DeleteAsync(string url)
        {
            if(url== "https://static.vecteezy.com/system/resources/thumbnails/009/292/244/small/default-avatar-icon-of-social-media-user-vector.jpg")
            {
                return;
            }

            if(url== MediaStandar.StandarProfileImage)
            {
                return;
            }
            string RootPath = _host.WebRootPath.Replace("\\\\", "\\");
            var mediaNameToDelete = Path.GetFileNameWithoutExtension(url);
            var EXT = Path.GetExtension(url);
            string? oldPath = "";
            if (IsImageExtension(EXT))
                oldPath = $@"{RootPath}\Images\{mediaNameToDelete}{EXT}";
            if (IsVideoExtension(EXT))
                oldPath = $@"{RootPath}\Videos\{mediaNameToDelete}{EXT}";
            else
            {
                throw new BadRequestException(_localization["DeleteMediaFail"].Value);
            }
            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
                return;
            }
            throw new BadRequestException(_localization["NotFoundMedia"].Value);
        }
        public async Task<string> UpdateAsync(string oldUrl, IFormFile newMedia)
        {
            return await AddAsync(newMedia)!;
        }
        public async Task<string>UpdateAsync(string oldUrl,MediaFile newMedia)
        {
            if (oldUrl == null && newMedia == null)
            {
                return "";
            }

            if (newMedia == null)
            {
                return oldUrl;
            }

            if (oldUrl == null)
            {
                return await AddAsync(newMedia)!;
            }

            await DeleteAsync(oldUrl);
            return await AddAsync(newMedia)!;
        }
    }
}
