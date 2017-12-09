using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public class LessCaloricQuestion : BaseQuestion
    {
        public override string Question => "Do you want to see less caloric food?";

        public LessCaloricQuestion(Recipe recipe) : base(recipe)
        {
            AddChoice("No, it's OK", (r, data) => 0);
            AddChoice("Yes, please", (r, data) => r.Calories < recipe.Calories ? 0.5 : 0);
        }
    }
}