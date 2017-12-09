using System.Collections.Generic;
using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public interface IQuestion
    {
        string Question { get; }

        string Data { get; }

        List<(int, string)> GetChoices();

        int CountChoices();

        string GetChoice(int index);

        List<(Recipe, double?)> EvaluateRecipes(List<Recipe> recipes, int choiceId, string data);
    }
}