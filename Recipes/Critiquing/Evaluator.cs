using RecipesCore.Models;

namespace RecipesCore.Critiquing
{
    public delegate double? Evaluator(Recipe recipe, string data);
}