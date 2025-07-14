using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class UserService(StoreContext _context) : IUserService
    {
        public async Task<bool> HasExistingReview(int bookId, string userId)
        {
            return await _context.Review.AnyAsync(r => r.BookId == bookId && r.UserId == userId);
        }
    }
}
