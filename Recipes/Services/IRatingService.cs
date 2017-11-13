using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public interface IRatingService
    {
        List<RecipeRatings> GetAll();

        List<RecipeRatings> GetByUserId(long userId);

        RecipeRatings GetByUserNameAndRecipeId(string name, long recipeId);

        RecipeRatings GetByUserIdAndRecipeId(long userId, long recipeId);

        List<RecipeRatings> GetByRecipeId(long recipeId);

        double GetAverageRatingForUser(long userId);

        double GetAverageRatingForRecipe(long recipeId);

        void Add(RecipeRatings rating);

        Boolean Exists(long recipeId, long userId);
    }
}
