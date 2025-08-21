using Domain.Entities.User;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser?> GetUserWithDetailsAsync(string userId);
    }
}
