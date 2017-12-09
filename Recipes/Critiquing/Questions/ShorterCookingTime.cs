using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public class ShorterCookingTime : BaseQuestion
    {
        public override string Question => "Do you prefer recipes with shorter cooking time?";

        public ShorterCookingTime(Recipe recipe) : base(recipe)
        {
            AddChoice("No, it's OK", (r, data) => 0);
            AddChoice("Yes, please", (r, data) => r.CookTime < recipe.CookTime ? 0.5 : 0);
        }
    }
}