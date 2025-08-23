using API.Extensions;
using Application.Dtos.CatalogDtos;
using Application.Interfaces.Services;
using Application.Specificatios.Params;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(IBookService bookService, ILogger<BookController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetBooks([FromQuery] BookSpecParams bookSpecParams)
        {
            logger.LogInformation("Fetching books with params: {@Params}", bookSpecParams);
            var result = await bookService.GetBooksAsync(bookSpecParams);
            return result.ToActionResult();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SingleBookDto>> GetBook(int id)
        {
            logger.LogInformation("Fetching book with id: {Id}", id);
            var result = await bookService.GetBookByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("{id:int}/rating")]
        public async Task<ActionResult<int>> GetBookRating(int id)
        {
            logger.LogInformation("Fetching rating for book with id: {Id}", id);
            var result = await bookService.GetBookRatingAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("rating-top")]
        public async Task<ActionResult<IReadOnlyList<CatalogBookDto>>> GetTopBooks()
        {
            logger.LogInformation("Fetching top rated books");
            var result = await bookService.GetTopRatedBooksAsync();
            return result.ToActionResult();
        }

        [HttpGet("newest")]
        public async Task<ActionResult<IReadOnlyList<CatalogBookDto>>> GetNewestBooks()
        {
            logger.LogInformation("Fetching newest books");
            var result = await bookService.GetNewestBooksAsync();
            return result.ToActionResult();
        }
    }
}
