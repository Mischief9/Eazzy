using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Application.Services.ImageService
{
    public interface IImageService
    {
        string Upload(IFormFile image);

        string GetImageUrlByName(string name);
    }
}
