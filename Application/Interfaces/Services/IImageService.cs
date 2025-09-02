using Application.Common.Result;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    public interface IImageService
    {
        Task<Result<string>> SaveImageAsync(IFormFile file, string folder = "images");
    }
}
