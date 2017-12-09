using System;
using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public static class QuestionFactory
    {
        public static QuestionFactory<T> For<T>() where T : BaseQuestion => new QuestionFactory<T>();
    }

    public class QuestionFactory<T> : IQuestionFactory
        where T : BaseQuestion
    {
        public IQuestion GetQuestion(Recipe recipe)
        {
            return (IQuestion) Activator.CreateInstance(typeof(T), recipe);
        }
    }
}