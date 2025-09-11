using API.Extensions;
using API.RequestHelpers;
using Application.Dtos.ReviewDtos;
using Application.Interfaces.Services;
using Application.Specificatios.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(ILogger<BookController> logger, IReviewService reviewService) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [Cache(600)]
        [HttpGet]
        public async Task<IActionResult> GetReviews([FromQuery] PaginationParams paginationParams)
        {
            logger.LogInformation("Fetching reviews with params: {@Params}", paginationParams);
            var result = await reviewService.GetReviewsAsync(paginationParams);
            return result.ToActionResult();
        }

        [Cache(600)]
        [HttpGet("book/{id:int}")]
        public async Task<IActionResult> GetBookReviews([FromQuery] PaginationParams paginationParams, int id)
        {
            logger.LogInformation("Fetching reviews with params: {@PaginationParams}, for book {BookId}", paginationParams, id);
            var result = await reviewService.GetBookReviewsAsync(id, paginationParams);
            return result.ToActionResult();
        }


        [Authorize]
        [InvalidateCache("api/adminbook", "api/book", "api/review")]
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewDto reviewdto)
        {
            logger.LogInformation("Attempting to create a new review for BookId {BookId} by UserId {UserId}", reviewdto.BookId, reviewdto.UserId);
            var userId = User.GetUserId();
            var result = await reviewService.CreateReviewAsync(reviewdto, userId);
            return result.ToActionResult();
        }

        [Authorize]
        [InvalidateCache("api/adminbook", "api/book", "api/review")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewDto dto)
        {
            logger.LogInformation("Attempting to update review with ID {ReviewId}", id);
            var userId = User.GetUserId();
            var result = await reviewService.UpdateReviewAsync(id, dto, userId);
            return result.ToActionResult();
        }

        [Authorize]
        [InvalidateCache("api/adminbook", "api/book", "api/review")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            logger.LogInformation("Attempting to delete review with ID {ReviewId}", id);
            var userId = User.GetUserId();
            var userRoles = User.GetUserRoles();
            var result = await reviewService.DeleteReviewAsync(id, userId, userRoles);
            return result.ToActionResult();
        }
    }
}
