using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public class LessIngredientsQuestion : BaseQuestion
    {
        public override string Question => "Do you prefer recipes with less ingredients?";

        public LessIngredientsQuestion(Recipe recipe) : base(recipe)
        {
            AddChoice("No, it's OK", (r, data) => 0);
            AddChoice("Yes, please", (r, data) => r.Ingredients.Count < recipe.Ingredients.Count ? 0.5 : 0);
        }
    }
}