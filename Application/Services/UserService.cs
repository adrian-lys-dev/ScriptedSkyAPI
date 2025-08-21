using Application.Dtos.UserProfileDtos;
using Application.Interfaces;
using Domain.Entities.OrderAggregate;

namespace Application.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<UserStatsDto?> GetUserStatsAsync(string userId)
        {
            var user = await userRepository.GetUserWithDetailsAsync(userId);
            if (user == null) return null;

            return new UserStatsDto
            {
                TotalOrders = user.Order.Count,
                ActiveOrders = user.Order.Count(o => o.Status == OrderStatus.Confirmed),
                TotalReviews = user.Reviews.Count,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> HasExistingReview(int bookId, string userId)
        {
            return await userRepository.ReviewExistsAsync(bookId, userId);
        }
    }
}
