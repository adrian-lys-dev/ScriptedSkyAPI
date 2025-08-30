using API.Extensions;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController(IDashboardService dashboardService) : ControllerBase
    {
        [HttpGet("card-stats")]
        public async Task<IActionResult> GetCardStats()
        {
            var result = await dashboardService.GetStatsAsync();
            return result.ToActionResult();
        }

        [HttpGet("monthly-sales/{year}")]
        public async Task<IActionResult> GetMonthlySales(int year)
        {
            var result = await dashboardService.GetMonthlySalesAsync(year);
            return result.ToActionResult();
        }

        [HttpGet("genres-sales/{top}")]
        public async Task<IActionResult> GetTopGenres(int top)
        {
            var result = await dashboardService.GetTopSellingGenresAsync(top);
            return result.ToActionResult();
        }
        [HttpGet("review-rating-distribution")]
        public async Task<IActionResult> GetReviewRatingDistribution()
        {
            var result = await dashboardService.GetReviewRatingDistributionAsync();
            return result.ToActionResult();
        }

        [HttpGet("avatar-usage")]
        public async Task<IActionResult> GetAvatarUsage()
        {
            var result = await dashboardService.GetAvatarUsageAsync();
            return result.ToActionResult();
        }
    }
}
