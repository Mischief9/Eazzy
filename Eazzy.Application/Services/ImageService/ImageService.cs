using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Eazzy.Application.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _basePath;

        public ImageService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _basePath = hostingEnvironment.WebRootPath + "/Images"; 
        }

        public string GetImageUrlByName(string name)
        {
            return $"https://localhost:44353/Images/{name}";
        }

        public string Upload(IFormFile image)
        {
            if (!File.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }

            using MemoryStream memoryStream = new MemoryStream();
            image.OpenReadStream().CopyTo(memoryStream);

            var guid = Guid.NewGuid();
            var fileName = $"{guid}{Path.GetExtension(image.FileName)}";
            var url = $"{_basePath}/{fileName}";

            using var fileStream = File.Create(url);

            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.CopyTo(fileStream);

            return fileName;
        }
    }
}
