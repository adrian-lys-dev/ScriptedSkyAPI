using Core.Entities;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser?> GetUserWithDetailsAsync(string userId);
    }
}
