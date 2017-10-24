using Microsoft.Extensions.DependencyInjection;
using RecipesCore.Services;

namespace RecipesCore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRecipesCore(this IServiceCollection sc)
        {
            sc.AddDbContext<RecipesContext>();

            sc.AddTransient<IRecipesService, RecipesService>();

            return sc;
        }
    }
}