using Application.Common;
using Application.Common.Result;
using Application.Dtos.ReviewDtos;
using Application.Specificatios.Params;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task<Result<Review>> CreateReviewAsync(ReviewDto reviewdto, string userId);
        Task<Result<Pagination<Review>>> GetReviewsAsync(PaginationParams paginationParams);
        Task<Result<Pagination<BookReviewDto>>> GetBookReviewsAsync(int bookId, PaginationParams paginationParams);
        Task<Result> UpdateReviewAsync(int id, ReviewDto dto, string userId);
        Task<Result> DeleteReviewAsync(int id, string userId, IEnumerable<string> userRoles);
    }
}
