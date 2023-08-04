using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Application.FileService
{
    public interface IFileService
    {
        /// <summary>
        /// Create Image
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        Task<string> CreateImage(IFormFile img);

        /// <summary>
        /// Upload Video
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        Task<string> UploadVideo(IFormFile media);


        /// <summary>
        /// Delete Image
        /// </summary>
        /// <param name="url"></param>
        Task<string> DeleteImage(string url);
    }
}
