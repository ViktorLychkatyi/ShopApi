
using ShopApi.Interfaces;

namespace ShopApi.Services
{
    public class ImageService(IWebHostEnvironment _environment) : IImageService
    {

        //private static string _dirname = "images"; // string.IsNullOrEmpty(dto.Url) ? "" : dto.Url // "categories" // "images"

        public async Task<string> SaveFileAsync(IFormFile file, string _dirname)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var folderPath = Path.Combine(_environment.WebRootPath, _dirname);
            Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var filePath = Path.Combine(folderPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }
    }
}
