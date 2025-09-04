using Application.Common.Result;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class ImageService(IWebHostEnvironment env, ILogger<ImageService> logger) : IImageService
    {
        public async Task<Result<string>> SaveImageAsync(IFormFile file, string folder = "images")
        {
            if (file == null || file.Length == 0)
            {
                logger.LogWarning("Attempted to save empty file");
                return Result<string>.Failure(new Error(ErrorType.BadRequest, "File is empty"));
            }
               
            try
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                if (!allowedExtensions.Contains(extension))
                {
                    logger.LogWarning("Invalid file format attempted: {Extension}", extension);
                    return Result<string>.Failure(new Error(ErrorType.BadRequest, "Invalid file format"));
                }      

                var fileName = Guid.NewGuid().ToString("N") + extension;

                var path = Path.Combine(env.WebRootPath, folder);
                if (!Directory.Exists(path))
                {
                    logger.LogInformation("Created folder for images at {Path}", path);
                    Directory.CreateDirectory(path);
                }             

                var filePath = Path.Combine(path, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                var url = $"/{folder}/{fileName}";
                logger.LogInformation("File saved successfully at {Url}", url);

                return Result<string>.SuccessResult(url);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving file {FileName}", file.FileName);
                return Result<string>.Failure(new Error(ErrorType.ServerError, $"Error saving file: {ex.Message}"));
            }
        }
    }
}
