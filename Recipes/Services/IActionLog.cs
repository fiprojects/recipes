using System.Collections.Generic;
using RecipesCore.Info;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public interface IActionLog
    {
        void LogDisplayedRecipe(Recipe recipe, string username, string referer, RecommendingAlgorithm algorithm);


        void LogRecommendedRecipes(Recipe recipe, List<Recipe> recommendedRecipes, string username, string referer, RecommendingAlgorithm algorithm);
    }
}