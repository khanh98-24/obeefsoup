using Microsoft.AspNetCore.Http;

namespace OBeefSoup.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string folder = "products")
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("No file uploaded");

            if (!IsValidImage(imageFile))
                throw new ArgumentException("Invalid image file");

            // Create folder if it doesn't exist
            var uploadPath = Path.Combine(_environment.WebRootPath, "images", folder);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Return relative URL
            return $"/images/{folder}/{fileName}";
        }

        public async Task<string> SaveImageFromBase64Async(string base64Data, string folder = "banners")
        {
            if (string.IsNullOrEmpty(base64Data))
                throw new ArgumentException("No image data");

            // Parse base64 data URL: "data:image/png;base64,xxxxx"
            var parts = base64Data.Split(',');
            if (parts.Length != 2)
                throw new ArgumentException("Invalid base64 format");

            var meta = parts[0]; // e.g. "data:image/png;base64"
            var data = parts[1];

            // Determine extension
            var ext = ".png";
            if (meta.Contains("image/jpeg") || meta.Contains("image/jpg")) ext = ".jpg";
            else if (meta.Contains("image/gif")) ext = ".gif";
            else if (meta.Contains("image/webp")) ext = ".webp";

            var uploadPath = Path.Combine(_environment.WebRootPath, "images", folder);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadPath, fileName);

            var bytes = Convert.FromBase64String(data);

            // Check file size (5MB)
            if (bytes.Length > _maxFileSize)
                throw new ArgumentException("File quá lớn (tối đa 5MB)");

            await File.WriteAllBytesAsync(filePath, bytes);

            return $"/images/{folder}/{fileName}";
        }

        public void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            // Skip if it's an external URL or placeholder
            if (imageUrl.StartsWith("http://") || imageUrl.StartsWith("https://"))
                return;

            var filePath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/'));
            
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception)
                {
                    // Log error but don't throw - deletion failure shouldn't stop the operation
                }
            }
        }

        public bool IsValidImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return false;

            // Check file size
            if (imageFile.Length > _maxFileSize)
                return false;

            // Check file extension
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return false;

            return true;
        }
    }
}
