using System.Collections.Generic;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public interface ICritiquingService
    {
        void Critique(string username, long recipeId, int questionId, int choiceId, string data = null);

        void Penalize(string username, Recipe recipe, double factor = 1);

        List<Recipe> GetRecommended(int limit, long? userId);
    }
}