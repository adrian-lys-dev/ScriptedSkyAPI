using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class UserService(StoreContext _context, IUserRepository userRepository) : IUserService
    {
        public async Task<UserStats?> GetUserStatsAsync(string userId)
        {
            var user = await userRepository.GetUserWithDetailsAsync(userId);
            if (user == null) return null;

            return new UserStats
            {
                TotalOrders = user.Order.Count,
                ActiveOrders = user.Order.Count(o => o.Status == OrderStatus.Confirmed),
                TotalReviews = user.Reviews.Count,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> HasExistingReview(int bookId, string userId)
        {
            return await _context.Review.AnyAsync(r => r.BookId == bookId && r.UserId == userId);
        }
    }
}
