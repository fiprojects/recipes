using RecipesCore.Models;

namespace RecipesWeb.ViewModels
{
    public class RecipesShowModel
    {
        public Recipe Recipe { get; set; }

        public RecipeRatings RecipeUserRating { get; set; }
    }
}
