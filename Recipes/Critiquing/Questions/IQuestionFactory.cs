using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public interface IQuestionFactory
    {
        IQuestion GetQuestion(Recipe recipe);
    }
}