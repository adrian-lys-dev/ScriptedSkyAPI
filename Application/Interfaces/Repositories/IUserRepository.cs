using Domain.Entities.User;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByIdAsync(string userId);
        Task<AppUser?> GetUserWithDetailsAsync(string userId);
        Task<bool> ReviewExistsAsync(int bookId, string userId);
        Task<IReadOnlyList<AppUser>> GetAllUsersAsync();
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<bool> RoleExistsAsync(string role);
        Task<bool> AddToRoleAsync(AppUser user, string role);
        Task<bool> RemoveFromRoleAsync(AppUser user, string role);
    }
}
