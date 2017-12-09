using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public class ShorterPreparationTime : BaseQuestion
    {
        public override string Question => "Do you prefer recipes with shorter preparation time?";

        public ShorterPreparationTime(Recipe recipe) : base(recipe)
        {
            AddChoice("No, it's OK", (r, data) => 0);
            AddChoice("Yes, please", (r, data) => r.PreparationTime < recipe.PreparationTime ? 0.5 : 0);
        }
    }
}