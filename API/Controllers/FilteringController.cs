using Application.Dtos.FilteringDtos;
using Application.Interfaces;
using Application.Mapping;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilteringController(IUnitOfWork unit, ILogger<FilteringController> logger) : ControllerBase
    {
        [HttpGet("genre")]
        public async Task<ActionResult<FilteringDto>> GetFilteringGenres()
        {
            logger.LogInformation("Requested list of genres");

            var genres = await unit.Repository<Genre>().ListAllAsync();

            var genreDtos = genres
                .Select(FilteringMapping.EntityToFilteringDto)
                .ToList();

            logger.LogInformation("Returned {Count} genres", genreDtos.Count);

            return Ok(genreDtos);
        }

        [HttpGet("author")]
        public async Task<ActionResult<FilteringDto>> GetFilteringAuthors()
        {
            logger.LogInformation("Requested list of authors");

            var authors = await unit.Repository<Author>().ListAllAsync();

            var authorDtos = authors
                .Select(FilteringMapping.EntityToFilteringDto)
                .ToList();

            logger.LogInformation("Returned {Count} authors", authorDtos.Count);

            return Ok(authorDtos);
        }

        [HttpGet("publisher")]
        public async Task<ActionResult<FilteringDto>> GetFilteringPublishers()
        {
            logger.LogInformation("Requested list of publishers");

            var publishers = await unit.Repository<Publisher>().ListAllAsync();

            var publisherDtos = publishers
                .Select(FilteringMapping.EntityToFilteringDto)
                .ToList();

            logger.LogInformation("Returned {Count} publishers", publisherDtos.Count);

            return Ok(publisherDtos);
        }
    }
}
