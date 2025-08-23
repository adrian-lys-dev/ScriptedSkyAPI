using API.Extensions;
using Application.Dtos.FilteringDtos;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilteringController(IFilteringService filteringService, ILogger<FilteringController> logger) : ControllerBase
    {
        [HttpGet("genre")]
        public async Task<ActionResult<FilteringDto>> GetFilteringGenres()
        {
            logger.LogInformation("Requested list of genres");
            var result = await filteringService.GetGenresAsync();
            return result.ToActionResult();
        }

        [HttpGet("author")]
        public async Task<ActionResult<FilteringDto>> GetFilteringAuthors()
        {
            logger.LogInformation("Requested list of authors");
            var result = await filteringService.GetAuthorsAsync();
            return result.ToActionResult();
        }

        [HttpGet("publisher")]
        public async Task<ActionResult<FilteringDto>> GetFilteringPublishers()
        {
            logger.LogInformation("Requested list of publishers");
            var result = await filteringService.GetPublishersAsync();
            return result.ToActionResult();
        }
    }
}
