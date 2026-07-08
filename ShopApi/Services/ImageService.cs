
using ShopApplication.Interfaces;

namespace ShopApi.Services
{
    public class ImageService(IWebHostEnvironment _environment) : IImageService
    {
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
            stream.Dispose();

            Console.WriteLine(filePath);

            return fileName;
        }

        public void DeleteFile(string file, string path)
        {
            var filePath = Path.Combine(_environment.WebRootPath, path, file);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
