﻿using Application.Interfaces.Services;
using Application.Services;
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

            return services;
        }
    }
}
