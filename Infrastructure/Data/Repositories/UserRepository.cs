using Application.Interfaces;
using Domain.Entities.User;
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

        public async Task<bool> ReviewExistsAsync(int bookId, string userId)
        {
            return await context.Review.AnyAsync(r => r.BookId == bookId && r.UserId == userId);
        }
    }
}
