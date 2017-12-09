using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public class CategoryQuestion : BaseQuestion
    {
        public override string Question => $"How do you like {Category.Name} recipes?";

        public Category Category => Recipe.Category;

        public CategoryQuestion(Recipe recipe) : base(recipe)
        {
            AddChoice("Not quite", (r, data) => r.Category == recipe.Category ? -0.5 : 0);
            AddChoice("Just OK", (r, data) => 0);
            AddChoice("Very much", (r, data) => r.Category == recipe.Category ? 0.5 : 0);
        }
    }
}