using Application.Dtos.DashboardDtos;
using Application.Interfaces.Repositories;
using Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class DashboardRepository(StoreContext context) : IDashboardRepository
    {
        public async Task<int> GetTotalActiveOrdersCount()
        {
            return await context.Order
                .Where(o => o.Status != OrderStatus.Cancelled && o.Status != OrderStatus.Done)
                .CountAsync();
        }

        public async Task<int> GetTotalBooksCount()
        {
            return await context.Book.CountAsync();
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await context.Users.CountAsync();
        }

        public async Task<List<MonthlySalesDto>> GetMonthlySalesAsync(int year)
        {
            var sales = await context.Order
                .Where(o => o.Status == OrderStatus.Done && o.CreatedAt.Year == year)
                .GroupBy(o => o.CreatedAt.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = Enumerable.Range(1, 12)
                .Select(m => new MonthlySalesDto
                {
                    Month = m,
                    SalesCount = sales.FirstOrDefault(s => s.Month == m)?.Count ?? 0
                })
                .ToList();

            return result;
        }

        public async Task<List<GenreSalesDto>> GetTopSellingGenresAsync(int top)
        {
            return await context.Order
                .Where(o => o.Status == OrderStatus.Done)
                .SelectMany(o => o.OrderItem)
                .SelectMany(oi => oi.Book.Genre.Select(g => new { Genre = g.Name, oi.Quantity }))
                .GroupBy(x => x.Genre)
                .Select(g => new GenreSalesDto
                {
                    GenreName = g.Key,
                    TotalSold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(g => g.TotalSold)
                .Take(top)
                .ToListAsync();
        }

        public async Task<List<RatingCountDto>> GetReviewRatingDistributionAsync()
        {
            var reviews = await context.Review
                .GroupBy(r => r.Rating)
                .Select(g => new RatingCountDto
                {
                    Rating = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            var allRatings = Enumerable.Range(1, 5)
                .Select(r => new RatingCountDto
                {
                    Rating = r,
                    Count = reviews.FirstOrDefault(x => x.Rating == r)?.Count ?? 0
                })
                .OrderBy(x => x.Rating)
                .ToList();

            return allRatings;
        }

        public async Task<List<AvatarUsageDto>> GetAvatarUsageAsync()
        {
            var data = await context.Users
                .GroupBy(u => u.Avatar.AvatarPath)
                .Select(g => new
                {
                    AvatarPath = g.Key,
                    UserCount = g.Count()
                })
                .OrderByDescending(x => x.UserCount)
                .ToListAsync();

            var result = data
                .Select(x =>
                {
                    var name = Path.GetFileNameWithoutExtension(x.AvatarPath).Replace("_", " ");
                    return new AvatarUsageDto
                    {
                        AvatarName = name,
                        UserCount = x.UserCount
                    };
                })
                .ToList();

            return result;
        }

    }
}
