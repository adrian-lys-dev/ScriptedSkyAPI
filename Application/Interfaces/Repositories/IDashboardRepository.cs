using Application.Dtos.DashboardDtos;

namespace Application.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalUsersCountAsync();
        Task<int> GetTotalBooksCount();
        Task<int> GetTotalActiveOrdersCount();
        Task<List<MonthlySalesDto>> GetMonthlySalesAsync(int year);
        Task<List<GenreSalesDto>> GetTopSellingGenresAsync(int top);
        Task<List<RatingCountDto>> GetReviewRatingDistributionAsync();
        Task<List<AvatarUsageDto>> GetAvatarUsageAsync();
    }
}
