using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public class MoreChallengingQuestion : BaseQuestion
    {
        public override string Question => "Do you want to try more challenging recipes?";

        public MoreChallengingQuestion(Recipe recipe) : base(recipe)
        {
            AddChoice("No, it's OK", (r, data) => 0);
            AddChoice("Yes, please", (r, data) => MoreChallengingPredicate(r, recipe) ? 0.5 : 0);
        }

        private bool MoreChallengingPredicate(Recipe recipe, Recipe referredRecipe)
        {
            return recipe.PreparationTime > referredRecipe.PreparationTime
                   || recipe.CookTime > referredRecipe.CookTime
                   || recipe.Ingredients.Count > referredRecipe.Ingredients.Count;
        }
    }
}