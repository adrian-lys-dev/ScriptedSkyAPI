using Domain.Models;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> HasExistingReview(int bookId, string userId);
        Task<UserStats?> GetUserStatsAsync(string userId);
    }
}
