using System;
using System.Collections.Generic;
using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public class Questions
    {
        private static readonly List<IQuestionFactory> QuestionFactories = new List<IQuestionFactory>
        {
            QuestionFactory.For<CategoryQuestion>(),
            QuestionFactory.For<LessCaloricQuestion>(),
            QuestionFactory.For<MoreCaloricQuestion>(),
            QuestionFactory.For<ShorterPreparationTime>(),
            QuestionFactory.For<ShorterCookingTime>(),
            QuestionFactory.For<LessIngredientsQuestion>(),
            QuestionFactory.For<ShorterRecipesQuestion>(),
            QuestionFactory.For<MoreChallengingQuestion>()
        };

        private readonly Recipe _recipe;

        public IQuestion this[int index] => index < QuestionFactories.Count ? QuestionFactories[index].GetQuestion(_recipe) : null;

        public Questions(Recipe recipe)
        {
            _recipe = recipe;
        }

        public (int, IQuestion) RandomQuestion()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var index = random.Next(0, QuestionFactories.Count);

            return (index, QuestionFactories[index].GetQuestion(_recipe));
        }
    }
}