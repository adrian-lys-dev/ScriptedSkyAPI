using Application.Common.Result;
using Application.Dtos.DashboardDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using System.Globalization;

namespace Application.Services.Admin
{
    public class DashboardService(IDashboardRepository dashboardRepository) : IDashboardService
    {
        public async Task<Result<CardStats>> GetStatsAsync()
        {
            var result = new CardStats
            {
                TotalActiveOrders = await dashboardRepository.GetTotalActiveOrdersCount(),
                TotalBooks = await dashboardRepository.GetTotalBooksCount(),
                TotalUsers = await dashboardRepository.GetTotalUsersCountAsync()
            };

            return Result<CardStats>.SuccessResult(result);
        }

        public async Task<Result<List<MonthlySalesChartDto>>> GetMonthlySalesAsync(int year)
        {
            var data = await dashboardRepository.GetMonthlySalesAsync(year);
            var enCulture = new CultureInfo("en-US");

            var result = data
                .Select(d => new MonthlySalesChartDto
                {
                    Month = enCulture.DateTimeFormat.GetMonthName(d.Month),
                    SalesCount = d.SalesCount
                })
                .ToList();

            return Result<List<MonthlySalesChartDto>>.SuccessResult(result);
        }

        public async Task<Result<List<GenreSalesDto>>> GetTopSellingGenresAsync(int top)
        {
            var result = await dashboardRepository.GetTopSellingGenresAsync(top);
            return Result<List<GenreSalesDto>>.SuccessResult(result);
        }

        public async Task<Result<List<RatingCountDto>>> GetReviewRatingDistributionAsync()
        {
            var result = await dashboardRepository.GetReviewRatingDistributionAsync();
            return Result<List<RatingCountDto>>.SuccessResult(result);
        }

        public async Task<Result<List<AvatarUsageDto>>> GetAvatarUsageAsync()
        {
            var result = await dashboardRepository.GetAvatarUsageAsync();
            return Result<List<AvatarUsageDto>>.SuccessResult(result);
        }
    }
}