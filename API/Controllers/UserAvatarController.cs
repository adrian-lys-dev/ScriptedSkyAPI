using API.Errors;
using API.Extensions;
using API.Mapping;
using Application.Dtos.UserProfileDtos;
using Application.Interfaces;
using Domain.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserAvatarController(SignInManager<AppUser> signInManager, IUnitOfWork unit, ILogger<UserAvatarController> logger) : ControllerBase
    {
        [HttpGet("available")]
        public async Task<ActionResult<IReadOnlyList<AvatarDto>>> GetAvailableAvatars()
        {
            var avatars = await unit.Repository<Avatar>().ListAllAsync();

            var result = avatars.Select(AvatarMapping.ToDto).ToList();

            return Ok(result);
        }

        [HttpPost("update/{avatarId}")]
        public async Task<ActionResult> UpdateAvatar(int avatarId)
        {
            var userId = User.GetUserId();

            logger.LogInformation("User {UserId} requests to update avatar to {AvatarId}", userId, avatarId);

            var avatar = await unit.Repository<Avatar>().GetByIdAsync(avatarId);
            if (avatar == null)
            {
                logger.LogWarning("Avatar with id {AvatarId} not found", avatarId);
                return BadRequest(new ApiResponse(400, $"Avatar with id {avatarId} does not exist."));
            }

            var user = await signInManager.UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                logger.LogWarning("User with id {UserId} not found during avatar update", userId);
                return BadRequest(new ApiResponse(400, "User not found. Please ensure you are authenticated."));
            }

            user.AvatarId = avatarId;
            user.Avatar = avatar;

            var result = await signInManager.UserManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                logger.LogError("Failed to update avatar for user {UserId}.", userId);
                return BadRequest(new ApiResponse(400, "Failed to update avatar."));
            }

            logger.LogInformation("User {UserId} successfully updated avatar to {AvatarId}", userId, avatarId);
            return Ok();
        }
    }
}
