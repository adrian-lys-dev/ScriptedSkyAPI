using API.Dtos.UserProfileDtos;
using API.Extensions;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Re_ABP_Backend.Errors;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController(IUserService userService, ILogger<UserProfileController> logger) : ControllerBase
    {
        [HttpGet("stats")]
        public async Task<ActionResult<UserStatsDto>> GetUserStats()
        {
            var userId = User.GetUserId();
            logger.LogInformation("Attempting to retrieve stats for user ID: {UserId}", userId);

            var stats = await userService.GetUserStatsAsync(userId);

            if (stats == null)
                return NotFound(new ApiResponse(404, "User not found"));

            logger.LogInformation("User stats retrieved successfully for user ID: {UserId}", userId);

            return Ok(stats);
        }
    }
}
