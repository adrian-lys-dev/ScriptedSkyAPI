using Application.Common.Result;
using Application.Dtos.DashboardDtos;

namespace Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<Result<CardStats>> GetStatsAsync();
        Task<Result<List<MonthlySalesChartDto>>> GetMonthlySalesAsync(int year);
        Task<Result<List<GenreSalesDto>>> GetTopSellingGenresAsync(int top);
        Task<Result<List<RatingCountDto>>> GetReviewRatingDistributionAsync();
        Task<Result<List<AvatarUsageDto>>> GetAvatarUsageAsync();
    }
}
