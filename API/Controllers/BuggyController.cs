using API.Errors;
using API.Extensions;
using Application.Common.Result;
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
            var result = Result.Failure(new Error(ErrorType.Unauthorized, "Unauthorized access"));
            return result.ToActionResult();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            logger.LogWarning("Bad request at {Path}", HttpContext.Request.Path);
            var result = Result.Failure(new Error(ErrorType.BadRequest, "Not a good request"));
            return result.ToActionResult();
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            logger.LogWarning("Resource not found at {Path}", HttpContext.Request.Path);
            var result = Result.Failure(new Error(ErrorType.NotFound, "Resource not found"));
            return result.ToActionResult();
        }

        [HttpGet("internalerror")]
        public IActionResult GetInternalError()
        {
            throw new InvalidOperationException("This is a test exception");
        }

        [HttpPost("validationerror")]
        public IActionResult GetValidationError([FromForm] FilteringDto test)
        {
            var result = Result.Failure(new Error(ErrorType.Validation, "Validation failed"));
            return result.ToActionResult();
        }

        [Authorize]
        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = Result<string>.SuccessResult("Hello " + name + " with id of " + id);
            return result.ToActionResult();
        }
    }
}
