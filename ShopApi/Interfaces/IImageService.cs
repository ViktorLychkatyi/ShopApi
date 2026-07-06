using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApi.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveFileAsync(IFormFile file, string _dirname);
    }
}
