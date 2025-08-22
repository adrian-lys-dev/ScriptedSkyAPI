using Application.Common.Result;
using Application.Dtos.UserProfileDtos;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<UserStatsDto>> GetUserStatsAsync(string userId);
    }
}
