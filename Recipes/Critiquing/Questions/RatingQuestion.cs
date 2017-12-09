using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public class RatingQuestion : BaseQuestion
    {
        public override string Question => "Do you prefer recipes with better rating?";

        public RatingQuestion(Recipe recipe) : base(recipe)
        {
            AddChoice("No, it's OK", (r, data) => 0);
            AddChoice("Yes, please", (r, data) => r.Rating > recipe.Rating ? 0.3 : 0);
        }
    }
}