using API.Extensions;
using Application.Common;
using Application.Dtos.IdentityDtos;
using Application.Interfaces.Services;
using Application.Specificatios.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController(IUserService userService, ILogger<AdminUserController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Pagination<UserInfoDto>>> GetUsers([FromQuery] PaginationParams paginationParams)
        {
            logger.LogInformation("Admin requested list of users with pagination: PageIndex={PageIndex}, PageSize={PageSize}",
                paginationParams.PageIndex, paginationParams.PageSize);

            var result = await userService.GetAllUsersAsync(paginationParams);
            return result.ToActionResult();
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddUserRole([FromQuery] UserRoleDto dto)
        {
            logger.LogInformation("Admin trying to add role '{Role}' to user {UserId}", dto.Role, dto.UserId);

            var result = await userService.AddUserRole(dto.UserId, dto.Role);
            return result.ToActionResult();
        }

        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveUserRole([FromQuery] UserRoleDto dto)
        {
            logger.LogInformation("Admin trying to remove role '{Role}' from user {UserId}", dto.Role, dto.UserId);

            var result = await userService.RemoveUserRole(dto.UserId, dto.Role);
            return result.ToActionResult();
        }
    }
}
