using Application.Common;
using Application.Common.Result;
using Application.Dtos.ReviewDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mapping;
using Application.Specificatios;
using Application.Specificatios.Params;
using Domain.Entities;

namespace Application.Services
{
    public class ReviewService(IUserRepository userRepository, IUnitOfWork unit) : IReviewService
    {
        public async Task<Result<Review>> CreateReviewAsync(ReviewDto reviewdto, string userId)
        {
            if (reviewdto.UserId != userId)
                return Result<Review>.Failure(new Error(ErrorType.Forbidden, "You cannot create a review for another user"));

            var user = await userRepository.GetUserWithDetailsAsync(reviewdto.UserId);
            if (user == null)
                return Result<Review>.Failure(new Error(ErrorType.NotFound, "User not found"));

            var exists = await userRepository.ReviewExistsAsync(reviewdto.BookId, reviewdto.UserId);
            if (exists)
                return Result<Review>.Failure(new Error(ErrorType.BadRequest, "User already has a review for this book"));

            var review = ReviewMapping.ToEntity(reviewdto);

            unit.Repository<Review>().Add(review);

            if (!await unit.Complete())
                return Result<Review>.Failure(new Error(ErrorType.BadRequest, "Problem creating the review"));

            return Result<Review>.SuccessResult(review);
        }

        public async Task<Result<Pagination<Review>>> GetReviewsAsync(PaginationParams paginationParams)
        {
            var spec = new ReviewSpecification(paginationParams);

            var reviews = await unit.Repository<Review>().ListWithSpecAsync(spec);
            var count = await unit.Repository<Review>().CountSpecAsync(spec);

            var pagination = new Pagination<Review>( paginationParams.PageIndex, paginationParams.PageSize, count, reviews);

            return Result<Pagination<Review>>.SuccessResult(pagination);
        }

        public async Task<Result<Pagination<BookReviewDto>>> GetBookReviewsAsync(int bookId, PaginationParams paginationParams)
        {
            var spec = new ReviewSpecification(paginationParams, bookId);
            var reviews = await unit.Repository<Review>().ListWithSpecAsync(spec);
            var count = await unit.Repository<Review>().CountSpecAsync(spec);

            var data = ReviewMapping.ToBookReviewDtoList(reviews);

            var pagination = new Pagination<BookReviewDto>(paginationParams.PageIndex, paginationParams.PageSize, count, data);

            return Result<Pagination<BookReviewDto>>.SuccessResult(pagination);
        }

        public async Task<Result> UpdateReviewAsync(int id, ReviewDto dto, string userId)
        {
            var existing = await unit.Repository<Review>().GetByIdAsync(id);

            if (existing is null)
                return Result.Failure(new Error(ErrorType.NotFound, "Review not found"));

            if (existing.UserId != userId)
                return Result.Failure(new Error(ErrorType.Forbidden, "You are not allowed to modify this review."));

            ReviewMapping.UpdateEntity(existing, dto);

            unit.Repository<Review>().Update(existing);

            if (!await unit.Complete())
                return Result.Failure(new Error(ErrorType.BadRequest, "Problem updating the review"));

            return Result.SuccessResult();
        }

        public async Task<Result> DeleteReviewAsync(int id, string userId, IEnumerable<string> userRoles)
        {
            var existing = await unit.Repository<Review>().GetByIdAsync(id);
            if (existing == null)
                return Result.Failure(new Error(ErrorType.NotFound, "Review not found"));

            var isOwner = existing.UserId == userId;
            var isAdmin = userRoles.Contains("Admin");

            if (!isOwner && !isAdmin)
                return Result.Failure(new Error(ErrorType.Forbidden, "You are not allowed to delete this review."));

            unit.Repository<Review>().Delete(existing);

            if (!await unit.Complete())
                return Result.Failure(new Error(ErrorType.BadRequest, "Problem deleting the review"));

            return Result.SuccessResult();
        }
    }
}
