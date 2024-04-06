using Microsoft.AspNetCore.Http;

namespace PersonDirectory.Interfaces.Services
{
    public interface IImageService
    {
        Task<string> GetImagePathAsync(int personId);
        Task<string> InsertImageAsync(int personId, IFormFile imageFile);
        Task<string> UpdateImageAsync(int personId, IFormFile imageFile);
        Task DeleteImageAsync(int personId);
    }
}
