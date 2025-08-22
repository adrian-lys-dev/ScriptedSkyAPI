using Application.Common.Result;
using Application.Dtos.UserProfileDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mapping;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserAvatarService(IUnitOfWork unit, UserManager<AppUser> userManager) : IUserAvatarService
    {
        public async Task<Result<IReadOnlyList<AvatarDto>>> GetAvailableAvatarsAsync()
        {
            var avatars = await unit.Repository<Avatar>().ListAllAsync();

            var avatarDtos = avatars.Select(AvatarMapping.ToDto).ToList();

            return Result<IReadOnlyList<AvatarDto>>.SuccessResult(avatarDtos);
        }

        public async Task<Result> UpdateAvatarAsync(string userId, int avatarId)
        {
            var avatar = await unit.Repository<Avatar>().GetByIdAsync(avatarId);
            if (avatar == null)
                return Result.Failure(new Error(ErrorType.NotFound, $"Avatar with id {avatarId} does not exist."));

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(new Error(ErrorType.Unauthorized, "User not found. Please ensure you are authenticated."));

            user.AvatarId = avatarId;
            user.Avatar = avatar;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Result.Failure(new Error(ErrorType.BadRequest, "Failed to update avatar."));
            }

            return Result.SuccessResult();
        }
    }
}
