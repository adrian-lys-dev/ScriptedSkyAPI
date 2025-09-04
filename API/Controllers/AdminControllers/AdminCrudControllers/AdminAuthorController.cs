using API.Extensions;
using Application.Common;
using Application.Dtos.AdminDtos.AuthorDtos;
using Application.Interfaces.Services;
using Application.Specificatios.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AdminControllers.AdminCrudControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAuthorController(IAdminAuthorService adminAuthorService, ILogger<AdminAuthorController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Pagination<AuthorDto>>> GetAuthors([FromQuery] PaginationParams paginationParams)
        {
            logger.LogInformation("Admin requested list of authors with pagination: PageIndex={PageIndex}, PageSize={PageSize}",
                paginationParams.PageIndex, paginationParams.PageSize);

            var result = await adminAuthorService.GetAllAuthorsAsync(paginationParams);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            logger.LogInformation("Admin requested author {Id}", id);

            var result = await adminAuthorService.GetAuthorByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor(CreateAuthorDto createAuthorDto)
        {
            logger.LogInformation("Admin requested creation of author with Name={Name}", createAuthorDto.Name);

            var result = await adminAuthorService.CreateAuthorAsync(createAuthorDto);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, CreateAuthorDto updateAuthorDto)
        {
            logger.LogInformation("Admin requested update of author with Id={Id}, new Name={Name}", id, updateAuthorDto.Name);

            var result = await adminAuthorService.UpdateAuthorAsync(updateAuthorDto, id);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            logger.LogInformation("Admin requested deletion of author with Id={Id}", id);

            var result = await adminAuthorService.DeleteAuthorAsync(id);
            return result.ToActionResult();
        }
    }
}
