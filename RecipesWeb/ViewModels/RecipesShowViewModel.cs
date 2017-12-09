using RecipesCore.Models;
using System.Collections.Generic;

namespace RecipesWeb.ViewModels
{
    public class RecipesShowModel
    {
        public Recipe Recipe { get; set; }

        public RecipeRatings RecipeUserRating { get; set; }

        public double AverageRating { get; set; }

        public List<Recipe> Recommended { get; set; }
        
        public List<Recipe> RecommendedByTfIdf { get; set; }
    }
}
