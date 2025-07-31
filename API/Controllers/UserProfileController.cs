using API.Dtos.UserProfileDtos;
using API.Extensions;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Re_ABP_Backend.Errors;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController(IUserRepository userRepository, ILogger<AccountController> logger) : ControllerBase
    {
        [HttpGet("stats")]
        public async Task<ActionResult<UserStatsDto>> GetUserStats()
        {
            var userId = User.GetUserId();

            var user = await userRepository.GetUserWithDetailsAsync(userId);

            if (user == null)
                return NotFound(new ApiResponse(404, "User not found"));

            var result = new UserStatsDto
            {
                TotalOrders = user.Order.Count,
                ActiveOrders = user.Order.Count(o => o.Status == OrderStatus.Confirmed),
                TotalReviews = user.Reviews.Count,
                CreatedAt = user.CreatedAt
            };

            return Ok(result);
        }

    }
}
