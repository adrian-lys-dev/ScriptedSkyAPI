using API.Extensions;
using API.RequestHelpers;
using Application.Common;
using Application.Dtos.AdminDtos.GenreDtos;
using Application.Interfaces.Services;
using Application.Specificatios.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AdminControllers.AdminCrudControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminGenreController(IAdminGenreService adminGenreService, ILogger<AdminGenreController> logger) : ControllerBase
    {
        [Cache(10000)]
        [HttpGet]
        public async Task<ActionResult<Pagination<GenreDto>>> GetGenres([FromQuery] PaginationParams paginationParams)
        {
            logger.LogInformation("Admin requested list of genres with pagination: PageIndex={PageIndex}, PageSize={PageSize}",
                paginationParams.PageIndex, paginationParams.PageSize);

            var result = await adminGenreService.GetAllGenresAsync(paginationParams);
            return result.ToActionResult();
        }

        [Cache(10000)]
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDto>> GetGenre(int id)
        {
            logger.LogInformation("Admin requested genre {Id}", id);

            var result = await adminGenreService.GetGenreByIdAsync(id);
            return result.ToActionResult();
        }

        [InvalidateCache("api/admingenre", "/api/filtering/genre")]
        [HttpPost]
        public async Task<IActionResult> CreateGenre(CreateGenreDto createGenreDto)
        {
            logger.LogInformation("Admin requested creation of genre with Name={Name}", createGenreDto.Name);

            var result = await adminGenreService.CreateGenreAsync(createGenreDto);
            return result.ToActionResult();
        }

        [InvalidateCache("api/admingenre", "/api/filtering/genre")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, CreateGenreDto updateGenreDto)
        {
            logger.LogInformation("Admin requested update of genre with Id={Id}, new Name={Name}", id, updateGenreDto.Name);

            var result = await adminGenreService.UpdateGenreAsync(updateGenreDto, id);
            return result.ToActionResult();
        }

        [InvalidateCache("api/admingenre", "/api/filtering/genre")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            logger.LogInformation("Admin requested deletion of genre with Id={Id}", id);

            var result = await adminGenreService.DeleteGenreAsync(id);
            return result.ToActionResult();
        }
    }
}
