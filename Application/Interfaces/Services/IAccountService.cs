using Application.Common.Result;
using Application.Dtos.IdentityDtos;
using System.Security.Claims;

namespace Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Result> LoginAsync(LoginDto loginDto);
        Task<Result> RegisterAsync(RegisterDto registerDto);
        Task<Result> LogoutAsync();
        Task<Result<UserInfoDto>> GetUserInfoAsync(ClaimsPrincipal user, string userEmail);
    }
}
