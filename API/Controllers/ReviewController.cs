using API.Dtos.ReviewDtos;
using API.Extensions;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specificatios;
using Core.Specificatios.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IUnitOfWork unit, ILogger<BookController> logger, IUserService userService) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Review>>> GetReviews([FromQuery] PaginationParams paginationParams)
        {

            logger.LogInformation("Fetching reviews with params: {@Params}", paginationParams);

            var spec = new ReviewSpecification(paginationParams);
            var reviews = await unit.Repository<Review>().ListWithSpecAsync(spec);
            var count = await unit.Repository<Review>().CountSpecAsync(spec);

            var pagination = new Pagination<Review>(paginationParams.PageIndex, paginationParams.PageSize, count, reviews);

            logger.LogInformation("Fetched {Count} reviews for page {PageIndex}", reviews.Count, paginationParams.PageIndex);

            return Ok(pagination);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewDto dto)
        {
            logger.LogInformation("Attempting to create a new review for BookId {BookId} by UserId {UserId}", dto.BookId, dto.UserId);

            var reviewExists = await userService.HasExistingReview(dto.BookId, dto.UserId);
            if (reviewExists)
            {
                logger.LogError("Failed to create review for BookId {BookId} by UserId {UserId}. User alredy has a review for this book", dto.BookId, dto.UserId);
                return BadRequest("User alredy has a review for this book");
            }

            var review = new Review
            {
                ReviewText = dto.ReviewText,
                Rating = dto.Rating,
                BookId = dto.BookId,
                UserId = dto.UserId,
            };

            unit.Repository<Review>().Add(review);


            if (await unit.Complete())
            {
                logger.LogInformation("Successfully created review with ID {ReviewId}", review.Id);
                return Ok(review);
            }

            logger.LogError("Failed to create review for BookId {BookId} by UserId {UserId}", dto.BookId, dto.UserId);
            return BadRequest("Problem creating the review");
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewDto dto)
        {
            logger.LogInformation("Attempting to update review with ID {ReviewId}", id);

            var userId = User.GetUserId();

            if (dto.UserId != userId)
            {
                logger.LogWarning("Unauthorized review update attempt. ReviewId: {ReviewId}, ProvidedUserId: {ProvidedUserId}, AuthenticatedUserId: {AuthenticatedUserId}",
                    id, dto.UserId, userId);
                return Forbid("You are not allowed to modify this review.");
            }

            var existing = await unit.Repository<Review>().GetByIdAsync(id);

            if (existing is null)
            {
                logger.LogWarning("Review with ID {ReviewId} not found", id);
                return NotFound("Review not found");
            }

            existing.ReviewText = dto.ReviewText;
            existing.Rating = dto.Rating;
            existing.BookId = dto.BookId;
            existing.UserId = dto.UserId;

            unit.Repository<Review>().Update(existing);

            if (await unit.Complete())
            {
                logger.LogInformation("Successfully updated review with ID {ReviewId}", id);
                return NoContent();
            }

            logger.LogError("Failed to update review with ID {ReviewId}", id);
            return BadRequest("Problem updating the review");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            logger.LogInformation("Attempting to delete review with ID {ReviewId}", id);

            var existing = await unit.Repository<Review>().GetByIdAsync(id);

            if (existing is null)
            {
                logger.LogWarning("Review with ID {ReviewId} not found", id);
                return NotFound("Review not found");
            }

            var userId = User.GetUserId();
            var userRoles = User.GetUserRole();

            var isOwner = existing.UserId == userId;
            var isAdmin = userRoles.Contains("Admin");

            if (!isOwner && !isAdmin)
            {
                logger.LogWarning("Unauthorized delete attempt. ReviewId: {ReviewId}, OwnerId: {OwnerId}, RequestingUserId: {UserId}, Roles: {@Roles}",
                    id, existing.UserId, userId, userRoles);

                return Forbid("You are not allowed to delete this review.");
            }

            unit.Repository<Review>().Delete(existing);

            if (await unit.Complete())
            {
                logger.LogInformation("Successfully deleted review with ID {ReviewId}", id);
                return NoContent();
            }

            logger.LogError("Failed to delete review with ID {ReviewId}", id);
            return BadRequest("Problem deleting the review");
        }
    }
}
