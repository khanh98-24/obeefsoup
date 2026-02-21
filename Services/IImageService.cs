using Microsoft.AspNetCore.Http;

namespace OBeefSoup.Services
{
    public interface IImageService
    {
        /// <summary>
        /// Save uploaded image to server
        /// </summary>
        /// <param name="imageFile">The uploaded image file</param>
        /// <param name="folder">Subfolder name (e.g., "products")</param>
        /// <returns>Relative URL path to the saved image</returns>
        Task<string> SaveImageAsync(IFormFile imageFile, string folder = "products");

        /// <summary>
        /// Delete image from server
        /// </summary>
        /// <param name="imageUrl">Relative URL of the image to delete</param>
        void DeleteImage(string imageUrl);

        /// <summary>
        /// Validate if the file is a valid image
        /// </summary>
        /// <param name="imageFile">The file to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        bool IsValidImage(IFormFile imageFile);

        /// <summary>
        /// Save cropped image from base64 data URL
        /// </summary>
        Task<string> SaveImageFromBase64Async(string base64Data, string folder = "banners");
    }
}
