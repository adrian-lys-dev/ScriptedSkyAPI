using API.Extensions;
using API.RequestHelpers;
using Application.Common;
using Application.Dtos.AdminDtos.PublisherDtos;
using Application.Interfaces.Services;
using Application.Specificatios.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AdminControllers.AdminCrudControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPublisherController(IAdminPublisherService adminPublisherService, ILogger<AdminPublisherController> logger) : ControllerBase
    {
        [Cache(10000)]
        [HttpGet]
        public async Task<ActionResult<Pagination<PublisherDto>>> GetPublishers([FromQuery] PaginationParams paginationParams)
        {
            logger.LogInformation("Admin requested list of publishers with pagination: PageIndex={PageIndex}, PageSize={PageSize}",
                paginationParams.PageIndex, paginationParams.PageSize);

            var result = await adminPublisherService.GetAllPublishersAsync(paginationParams);
            return result.ToActionResult();
        }

        [Cache(10000)]
        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDto>> GetPublisher(int id)
        {
            logger.LogInformation("Admin requested publisher {Id}", id);

            var result = await adminPublisherService.GetPublisherByIdAsync(id);
            return result.ToActionResult();
        }

        [InvalidateCache("api/adminpublisher", "/api/filtering/publisher")]
        [HttpPost]
        public async Task<IActionResult> CreatePublisher(CreatePublisherDto createPublisherDto)
        {
            logger.LogInformation("Admin requested creation of publisher with Name={Name}", createPublisherDto.Name);

            var result = await adminPublisherService.CreatePublisherAsync(createPublisherDto);
            return result.ToActionResult();
        }

        [InvalidateCache("api/adminpublisher", "/api/filtering/publisher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, CreatePublisherDto createPublisherDto)
        {
            logger.LogInformation("Admin requested update of publisher with Id={Id}, new Name={Name}", id, createPublisherDto.Name);

            var result = await adminPublisherService.UpdatePublisherAsync(createPublisherDto, id);
            return result.ToActionResult();
        }

        [InvalidateCache("api/adminpublisher", "/api/filtering/publisher")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            logger.LogInformation("Admin requested deletion of publisher with Id={Id}", id);

            var result = await adminPublisherService.DeletePublisherAsync(id);
            return result.ToActionResult();
        }
    }
}
