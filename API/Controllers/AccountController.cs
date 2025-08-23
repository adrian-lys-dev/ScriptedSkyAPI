using API.Extensions;
using Application.Dtos.IdentityDtos;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService accountService, ILogger<AccountController> logger) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            logger.LogInformation("Login attempt for email {Email}", loginDto.Email);
            var result = await accountService.LoginAsync(loginDto);
            return result.ToActionResult();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            logger.LogInformation("Registration attempt for email {Email}", registerDto.Email);
            var result = await accountService.RegisterAsync(registerDto);
            return result.ToActionResult();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            logger.LogInformation("Logout attempt");
            var result = await accountService.LogoutAsync();
            return result.ToActionResult();
        }

        [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo()
        {
            var result = await accountService.GetUserInfoAsync(User, User.GetEmail());
            return result.ToActionResult();
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