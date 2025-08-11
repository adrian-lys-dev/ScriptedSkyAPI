using API.Dtos.ReviewDtos;
using API.Helpers;
using Core.Entities;

namespace API.Mapping
{
    public static class ReviewMapping
    {
        public static BookReviewDto ToBookReviewDto(Review review)
        {
            return new BookReviewDto
            {
                Id = review.Id,
                ReviewText = review.ReviewText,
                Rating = review.Rating,
                UserId = review.UserId,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
                AvatarPath = UrlHelper.BuildImageUrl(review.AppUser.Avatar.AvatarPath),
                FirstName = review.AppUser.FirstName,
                LastName = review.AppUser.LastName
            };
        }

        public static IReadOnlyList<BookReviewDto> ToBookReviewDtoList(IEnumerable<Review> reviews)
        {
            return reviews.Select(ToBookReviewDto).ToList();
        }

        public static Review ToEntity(ReviewDto dto)
        {
            return new Review
            {
                ReviewText = dto.ReviewText,
                Rating = dto.Rating,
                BookId = dto.BookId,
                UserId = dto.UserId
            };
        }

        public static void UpdateEntity(Review existing, ReviewDto dto)
        {
            existing.ReviewText = dto.ReviewText;
            existing.Rating = dto.Rating;
            existing.BookId = dto.BookId;
            existing.UserId = dto.UserId;
        }

    }
}
