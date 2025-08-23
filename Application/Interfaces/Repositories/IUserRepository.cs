using Domain.Entities.User;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser?> GetUserWithDetailsAsync(string userId);
        Task<bool> ReviewExistsAsync(int bookId, string userId);
    }
}
