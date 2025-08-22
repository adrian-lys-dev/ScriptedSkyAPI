using Application.Common.Result;
using Application.Dtos.UserProfileDtos;

namespace Application.Interfaces.Services
{
    public interface IUserAvatarService
    {
        Task<Result<IReadOnlyList<AvatarDto>>> GetAvailableAvatarsAsync();
        Task<Result> UpdateAvatarAsync(string userId, int avatarId);
    }
}
