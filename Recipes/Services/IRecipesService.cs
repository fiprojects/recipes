using System.Collections.Generic;
using RecipesCore.Models;
using System;

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

        List<Recipe> GetRecommendedByIngredience(long recipeId, long? userId);

        List<Recipe> GetMRecipesFromNBestRated(int m = 12, int n = 50);
    }
}