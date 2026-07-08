using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveFileAsync(IFormFile file, string _dirname);
        void DeleteFile(string file, string path);
    }
}
