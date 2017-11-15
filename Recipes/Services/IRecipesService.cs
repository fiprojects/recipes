using System.Collections.Generic;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public interface IRecipesService
    {
        List<Recipe> GetAll();

        List<Recipe> GetAllByCategoryId(long id);

        Recipe Get(long id);

        void Add(Recipe recipe);

        List<Recipe> GetTopRecommended();

        List<Recipe> GetRecommendedByCategoryId(long id);
    }
}