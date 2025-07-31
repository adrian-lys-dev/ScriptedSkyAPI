using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository(StoreContext context) : IUserRepository
    {

        public async Task<AppUser?> GetUserWithDetailsAsync(string userId)
        {
            return await context.Users
                .Include(u => u.Order)
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

    }
}
