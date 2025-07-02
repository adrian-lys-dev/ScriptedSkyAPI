using API.Dtos.FilteringDtos;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
            return Unauthorized();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            logger.LogWarning("Bad request at {Path}", HttpContext.Request.Path);
            return BadRequest("Bad Request");
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            logger.LogWarning("Resource not found at {Path}", HttpContext.Request.Path);
            return NotFound("Resource not found");
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
    }
}
