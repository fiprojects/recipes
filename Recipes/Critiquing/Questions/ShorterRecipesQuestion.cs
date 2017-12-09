using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public class ShorterRecipesQuestion : BaseQuestion
    {
        public override string Question => "Do you prefer shorter recipes?";

        public ShorterRecipesQuestion(Recipe recipe) : base(recipe)
        {
            AddChoice("No, it's OK", (r, data) => 0);
            AddChoice("Yes, please", (r, data) => r.Directions.Length < recipe.Directions.Length ? 0.5 : 0);
        }
    }
}