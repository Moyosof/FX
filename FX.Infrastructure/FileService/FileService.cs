using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using FX.Application.FileService;
using FX.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Infrastructure.FileService
{
    public class FileService : IFileService
    {
        private readonly CloudinarySettings _cloudinarySettings;
        private readonly Cloudinary _cloudinary;
        public FileService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            _cloudinarySettings = cloudinarySettings.Value;
            Account account = new Account
            (
                 _cloudinarySettings.CloudName,
                 _cloudinarySettings.ApiKey,
                 _cloudinarySettings.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> CreateImage(IFormFile img)
        {
            try
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(img.FileName, img.OpenReadStream()),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                string url = uploadResult.SecureUrl.ToString();
                return url;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UploadVideo(IFormFile media)
        {
            try
            {
                var uploadParams = new VideoUploadParams // or ImageUploadParams depending on the file type
                {
                    File = new FileDescription(media.FileName, media.OpenReadStream()),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                    // Add any other transformations or options you need for the media
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                string url = uploadResult.SecureUrl.ToString();
                return url;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<string> DeleteImage(string url)
        {
            var publicId = GetPublicIdFromCloudinaryUrl(url);

            try
            {
                var deletionParams = new DeletionParams(url);
                var result = await _cloudinary.DestroyAsync(deletionParams);

                if (result.Result == "ok")
                {
                    // Image successfully deleted from Cloudinary
                    return string.Empty;
                }
                else
                {
                    // throw new Exception("Failed to delete the image from Cloudinary.");
                    return "Failed to delete the image from Cloudinary.";
                }
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }
        }

        private string GetPublicIdFromCloudinaryUrl(string url)
        {
            var uri = new Uri(url);
            var segments = uri.Segments;
            var publicId = segments.LastOrDefault()?.Split('.').FirstOrDefault();
            return publicId;
        }
    }
}
