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
    public class UserAvatarController(IUserAvatarService avatarService, ILogger<UserAvatarController> logger) : ControllerBase
    {
        [HttpGet("available")]
        public async Task<ActionResult<IReadOnlyList<AvatarDto>>> GetAvailableAvatars()
        {
            logger.LogInformation("Fetching available avatars...");
            var result = await avatarService.GetAvailableAvatarsAsync();
            return result.ToActionResult();
        }

        [HttpPost("update/{avatarId}")]
        public async Task<ActionResult> UpdateAvatar(int avatarId)
        {
            var userId = User.GetUserId();

            logger.LogInformation("User {UserId} requests to update avatar to {AvatarId}", userId, avatarId);

            var result = await avatarService.UpdateAvatarAsync(userId, avatarId);
            return result.ToActionResult();
        }
    }
}
