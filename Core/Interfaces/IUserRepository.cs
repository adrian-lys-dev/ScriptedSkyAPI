using Core.Entities.User;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser?> GetUserWithDetailsAsync(string userId);
    }
}
