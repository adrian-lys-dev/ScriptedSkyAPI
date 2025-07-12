using API.Dtos.IdentityDtos;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(SignInManager<AppUser> signInManager, ILogger<AccountController> logger) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    logger.LogWarning("Registration failed for {Email}. Code: {Code}, Description: {Description}",
                        registerDto.Email, error.Code, error.Description);

                    ModelState.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem();
            }

            logger.LogInformation("Registration succeeded for email: {Email}", registerDto.Email);
            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            logger.LogInformation("User logged out.");

            return NoContent();
        }

        [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == false) 
            {
                logger.LogInformation("Unauthenticated request to /user-info");
                return NoContent();
            } 

            var user = await signInManager.UserManager.GetUserByEmail(User);
            logger.LogInformation("User info retrieved for: {Email}", user.Email);

            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email
            });
        }

        [HttpGet]
        public ActionResult GetAuthState()
        {
            return Ok(new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated ?? false
            });
        }
    }

}