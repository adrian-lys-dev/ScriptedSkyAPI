using Application.Common;
using Application.Common.Result;
using Application.Dtos.IdentityDtos;
using Application.Dtos.UserProfileDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mapping;
using Application.Specificatios.Params;
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

        public async Task<Result<Pagination<UserInfoDto>>> GetAllUsersAsync(PaginationParams paginationParams)
        {
            var users = await userRepository.GetAllUsersAsync();
            var count = users.Count;

            var pagedUsers = users
                .Skip((paginationParams.PageIndex - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToList();

            var userDtos = new List<UserInfoDto>();

            foreach (var user in pagedUsers)
            {
                var roles = await userRepository.GetUserRolesAsync(user.Id);
                var dto = UserMapper.ToUserInfoDto(user, roles);
                userDtos.Add(dto);
            }

            var pagination = new Pagination<UserInfoDto>(
                paginationParams.PageIndex,
                paginationParams.PageSize,
                count,
                userDtos
            );

            return Result<Pagination<UserInfoDto>>.SuccessResult(pagination);
        }


        public async Task<Result> RemoveUserRole(string userId, string role)
        {
            var roleExist = await userRepository.RoleExistsAsync(role);

            if (!roleExist)
                return Result.Failure(new Error(ErrorType.BadRequest, "Role not found"));

            if (role == "User")
                return Result.Failure(new Error(ErrorType.BadRequest, "Cannot remove basic user role"));

            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
                return Result.Failure(new Error(ErrorType.NotFound, "User not found"));

            var result = await userRepository.RemoveFromRoleAsync(user, role);

            if (result)
                return Result.SuccessResult();
            else
                return Result.Failure(new Error(ErrorType.BadRequest, "Error removing role"));

        }

        public async Task<Result> AddUserRole(string userId, string role)
        {
            var roleExist = await userRepository.RoleExistsAsync(role);

            if(!roleExist)
                return Result.Failure(new Error(ErrorType.BadRequest, "Role not found"));

            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
                return Result.Failure(new Error(ErrorType.NotFound, "User not found"));

            var roles = await userRepository.GetUserRolesAsync(user.Id);
            if(roles.Contains(role))
                return Result.Failure(new Error(ErrorType.BadRequest, "The user already has this role"));

            var result = await userRepository.AddToRoleAsync(user, role);

            if (result)
                return Result.SuccessResult();
            else
                return Result.Failure(new Error(ErrorType.BadRequest, "Error adding role"));
        }
    }
}
