using Application.Interfaces.Repositories;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository(StoreContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) : IUserRepository
    {

        public async Task<IReadOnlyList<AppUser>> GetAllUsersAsync()
        {
            return await context.Users
                .Include(x => x.Avatar)
                .ToListAsync();
        }

        public async Task<AppUser?> GetByIdAsync(string userId)
            => await context.Users
                .Include(x => x.Avatar)
                .FirstOrDefaultAsync(x => x.Id == userId);

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return new List<string>();

            var roles = await userManager.GetRolesAsync(user);
            return roles;
        }

        public async Task<AppUser?> GetUserWithDetailsAsync(string userId)
        {
            return await context.Users
                .Include(u => u.Order)
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> AddToRoleAsync(AppUser user, string role)
        {
            var result = await userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> RemoveFromRoleAsync(AppUser user, string role)
        {
            var result = await userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> ReviewExistsAsync(int bookId, string userId)
        {
            return await context.Review.AnyAsync(r => r.BookId == bookId && r.UserId == userId);
        }

        public async Task<bool> RoleExistsAsync(string role)
            => await roleManager.RoleExistsAsync(role);

    }
}
