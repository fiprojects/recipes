using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public class MoreCaloricQuestion : BaseQuestion
    {
        public override string Question => "Do you want to see more caloric food?";

        public MoreCaloricQuestion(Recipe recipe) : base(recipe)
        {
            AddChoice("No, it's OK", (r, data) => 0);
            AddChoice("Yes, please", (r, data) => r.Calories > recipe.Calories ? 0.5 : 0);
        }
    }
}