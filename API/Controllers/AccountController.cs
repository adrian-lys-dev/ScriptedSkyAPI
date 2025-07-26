using API.Dtos.IdentityDtos;
using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Re_ABP_Backend.Errors;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(SignInManager<AppUser> signInManager, ILogger<AccountController> logger) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var user = await signInManager.UserManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                logger.LogWarning("Login failed. Invalid password for: {Email}", loginDto.Email);
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            logger.LogInformation("Login succeeded for {Email}", loginDto.Email);

            return NoContent();
        }


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
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                Roles = User.FindFirstValue(ClaimTypes.Role)
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