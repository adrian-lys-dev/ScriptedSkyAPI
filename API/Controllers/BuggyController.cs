using API.Errors;
using Application.Dtos.FilteringDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController(ILogger<BuggyController> logger) : ControllerBase
    {
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            logger.LogError("Unauthorized access attempt at {Path}", HttpContext.Request.Path);
            return Unauthorized(new ApiResponse(401));
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            logger.LogWarning("Bad request at {Path}", HttpContext.Request.Path);
            return BadRequest(new ApiResponse(400, "Not a good request"));
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            logger.LogWarning("Resource not found at {Path}", HttpContext.Request.Path);
            return NotFound(new ApiResponse(404));
        }

        [HttpGet("internalerror")]
        public IActionResult GetInternalError()
        {
            throw new InvalidOperationException("This is a test exception");
        }

        [HttpPost("validationerror")]
        public IActionResult GetValidationError([FromForm] FilteringDto test)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok("Hello " + name + " with id of " + id);
        }
    }
}
