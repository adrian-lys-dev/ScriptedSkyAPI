using Application.Interfaces.Services;
using Application.Services;
using Application.Services.Admin;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) 
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAvatarService, UserAvatarService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IFilteringService, FilteringService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAdminOrderService, AdminOrderService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IAdminGenreService, AdminGenreService>();
            services.AddScoped<IAdminAuthorService, AdminAuthorService>();

            return services;
        }
    }
}
