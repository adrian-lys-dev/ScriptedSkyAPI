using API.Extensions;
using Application.Common;
using Application.Dtos.GenreDtos;
using Application.Interfaces.Services;
using Application.Specificatios.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AdminControllers.AdminCrudControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminGenreController(IAdminGenreService adminGenreService, ILogger<OrderController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Pagination<GenreDto>>> GetGenres([FromQuery] PaginationParams paginationParams)
        {
            logger.LogInformation("Admin requested list of genres with pagination: PageIndex={PageIndex}, PageSize={PageSize}",
                paginationParams.PageIndex, paginationParams.PageSize);

            var result = await adminGenreService.GetAllGenresAsync(paginationParams);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDto>> GetGenre(int id)
        {
            logger.LogInformation("Admin requested genre {id}", id);
            var result = await adminGenreService.GetGenreByIdAsync(id);
            return result.ToActionResult();
        }
    }
}
