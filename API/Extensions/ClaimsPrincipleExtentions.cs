using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtentions
    {
        public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var userToReturn = await userManager.Users.
                FirstOrDefaultAsync(x => x.Email == user.GetEmail());

            if (userToReturn == null) throw new AuthenticationException("User not found");

            return userToReturn;
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email)
                ?? throw new AuthenticationException("Email claim not found");

            return email;
        }

        public static async Task<AppUser> GetUserAsync(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var userId = user.GetUserId();
            var appUser = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (appUser == null)
                throw new AuthenticationException("User not found");

            return appUser;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new AuthenticationException("User ID claim not found");
        }

        public static string GetUserRole(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role)
                ?? throw new AuthenticationException("User role claim not found");
        }

    }
}
