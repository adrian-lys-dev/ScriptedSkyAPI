using Application.Common.Result;
using Application.Dtos.UserProfileDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities.OrderAggregate;

namespace Application.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<Result<UserStatsDto>> GetUserStatsAsync(string userId)
        {
            var user = await userRepository.GetUserWithDetailsAsync(userId);
            if (user == null)
                return Result<UserStatsDto>.Failure(new Error(ErrorType.NotFound, "User not found"));

            var stats = new UserStatsDto
            {
                TotalOrders = user.Order.Count,
                ActiveOrders = user.Order.Count(o => o.Status == OrderStatus.Confirmed),
                TotalReviews = user.Reviews.Count,
                CreatedAt = user.CreatedAt
            };

            return Result<UserStatsDto>.SuccessResult(stats);
        }
    }
}
