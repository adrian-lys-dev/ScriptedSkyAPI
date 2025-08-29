using Application.Common;
using Application.Common.Result;
using Application.Dtos.IdentityDtos;
using Application.Dtos.UserProfileDtos;
using Application.Specificatios.Params;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<UserStatsDto>> GetUserStatsAsync(string userId);
        Task<Result<Pagination<UserInfoDto>>> GetAllUsersAsync(PaginationParams paginationParams);
        Task<Result> AddUserRole(string userId, string role);
        Task<Result> RemoveUserRole(string userId, string role);
    }
}
