using API.Extensions;
using Application.Dtos.UserProfileDtos;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            var result = await userService.GetUserStatsAsync(userId);

            return result.ToActionResult();
        }
    }
}
