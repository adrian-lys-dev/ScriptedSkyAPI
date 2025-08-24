using Application.Common.Result;
using Application.Dtos.IdentityDtos;
using Application.Helpers;
using Application.Interfaces.Services;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class AccountService(SignInManager<AppUser> signInManager, ILogger<AccountService> logger) : IAccountService
    {
        public async Task<Result> LoginAsync(LoginDto loginDto)
        {
            var user = await signInManager.UserManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Result.Failure(new Error(ErrorType.Unauthorized, "Invalid email or password"));

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                logger.LogWarning("Login failed. Invalid password for: {Email}", loginDto.Email);
                return Result.Failure(new Error(ErrorType.Unauthorized, "Invalid email or password"));
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            logger.LogInformation("Login succeeded for {Email}", loginDto.Email);
            return Result.SuccessResult();
        }

        public async Task<Result> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);
            await signInManager.UserManager.AddToRoleAsync(user, "User");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    logger.LogWarning("Registration failed for {Email}. Code: {Code}, Description: {Description}",
                        registerDto.Email, error.Code, error.Description);
                }

                return Result.Failure(new Error(ErrorType.Validation, "Registration failed"));
            }

            logger.LogInformation("Registration succeeded for email: {Email}", registerDto.Email);
            return Result.SuccessResult();
        }

        public async Task<Result> LogoutAsync()
        {
            await signInManager.SignOutAsync();
            logger.LogInformation("User logged out.");
            return Result.SuccessResult();
        }

        public async Task<Result<UserInfoDto>> GetUserInfoAsync(ClaimsPrincipal user)
        {
            if (user.Identity?.IsAuthenticated != true)
                return Result<UserInfoDto>.NoContent();

            var emailClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var email = emailClaim?.Value;
            if (email == null)
                return Result<UserInfoDto>.NoContent();

            var currentUser = await signInManager.UserManager.Users
                .Include(u => u.Avatar)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (currentUser == null)
            {
                return Result<UserInfoDto>.Failure(new Error(ErrorType.NotFound, "User not found"));
            }

            var dto = new UserInfoDto
            {
                Id = currentUser.Id,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email!,
                Avatar = UrlHelper.BuildImageUrl(currentUser.Avatar.AvatarPath),
                Roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList()
            };

            logger.LogInformation("User info retrieved for: {Email}", currentUser.Email);
            return Result<UserInfoDto>.SuccessResult(dto);
        }
    }
}
