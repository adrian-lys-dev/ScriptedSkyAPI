using API.Dtos.FilteringDtos;
using API.Mapping;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilteringController(IUnitOfWork unit) : ControllerBase
    {
        [HttpGet("genre")]
        public async Task<ActionResult<FilteringDto>> GetFilteringGenres()
        {
            var genres = await unit.Repository<Genre>().ListAllAsync();

            var genreDtos = genres
                .Select(FilteringMapping.EntityToFilteringDto)
                .ToList();

            return Ok(genreDtos);
        }

        [HttpGet("author")]
        public async Task<ActionResult<FilteringDto>> GetFilteringAuthors()
        {
            var authors = await unit.Repository<Author>().ListAllAsync();

            var authorDtos = authors
                .Select(FilteringMapping.EntityToFilteringDto)
                .ToList();

            return Ok(authorDtos);
        }

        [HttpGet("publisher")]
        public async Task<ActionResult<FilteringDto>> GetFilteringPublishers()
        {
            var publishers = await unit.Repository<Publisher>().ListAllAsync();

            var publisherDtos = publishers
                .Select(FilteringMapping.EntityToFilteringDto)
                .ToList();

            return Ok(publisherDtos);
        }
    }
}
