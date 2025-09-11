using API.Extensions;
using API.RequestHelpers;
using Application.Common;
using Application.Dtos.BookDtos;
using Application.Interfaces.Services;
using Application.Specificatios.Params;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AdminControllers.AdminCrudControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBookController(IAdminBookService adminBookService, ILogger<AdminBookController> logger) : ControllerBase
    {
        [Cache(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<BookDto>>> GetBooks([FromQuery] PaginationParams paginationParams)
        {
            logger.LogInformation("Admin requested list of books with pagination: PageIndex={PageIndex}, PageSize={PageSize}",
                paginationParams.PageIndex, paginationParams.PageSize);

            var result = await adminBookService.GetAllBooksAsync(paginationParams);
            return result.ToActionResult();
        }

        [Cache(600)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
            logger.LogInformation("Fetching book with id: {Id}", id);
            var result = await adminBookService.GetBookByIdAsync(id);
            return result.ToActionResult();
        }

        [InvalidateCache("api/adminbook", "api/book")]
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromForm] CreateBookDto createBookDto)
        {
            logger.LogInformation("Attempting to create book: Title={Title}", createBookDto.Title);
            var result = await adminBookService.CreateBookAsync(createBookDto);
            return result.ToActionResult();
        }

        [InvalidateCache("api/adminbook", "api/book")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Book>> UpdateBook([FromForm] CreateBookDto createBookDto, int id)
        {
            logger.LogInformation("Attempting to update book: Title={Title}", createBookDto.Title);
            var result = await adminBookService.UpdateBookAsync(id, createBookDto);
            return result.ToActionResult();
        }

        [InvalidateCache("api/adminbook", "api/book")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            logger.LogInformation("Attempting to delete book with Id={Id}", id);
            var result = await adminBookService.DeleteBookAsync(id);
            return result.ToActionResult();
        }

    }
}
