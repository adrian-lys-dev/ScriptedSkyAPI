using Application.Dtos.UserProfileDtos;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> HasExistingReview(int bookId, string userId);
        Task<UserStatsDto?> GetUserStatsAsync(string userId);
    }
}
