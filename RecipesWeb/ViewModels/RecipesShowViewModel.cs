using RecipesCore.Models;
using System.Collections.Generic;
using RecipesCore.Critiquing.Questions;

namespace RecipesWeb.ViewModels
{
    public class RecipesShowModel
    {
        public Recipe Recipe { get; set; }

        public RecipeRatings RecipeUserRating { get; set; }

        public double AverageRating { get; set; }

        public List<Recipe> Recommended { get; set; }

        public int CritiquingQuestionIndex { get; set; }

        public IQuestion CritiquingQuestion { get; set; }
    }
}
