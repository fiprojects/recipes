﻿using Microsoft.Extensions.DependencyInjection;
using RecipesCore.Services;

namespace RecipesCore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRecipesCore(this IServiceCollection sc)
        {
            sc.AddDbContext<RecipesContext>();

            sc.AddTransient<ICategoryService, CategoryService>();
            sc.AddTransient<IRecipesService, RecipesService>();
            sc.AddTransient<IUserService, UserService>();
            sc.AddTransient<IRatingService, RatingService>();

            return sc;
        }
    }
}