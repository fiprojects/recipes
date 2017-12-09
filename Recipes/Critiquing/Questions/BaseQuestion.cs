using System.Collections.Generic;
using System.Linq;
using RecipesCore.Models;

namespace RecipesCore.Critiquing.Questions
{
    public abstract class BaseQuestion : IQuestion
    {
        protected readonly List<Choice> Choices = new List<Choice>();
        protected readonly Recipe Recipe;

        public abstract string Question { get; }

        public string Data { get; protected set; }

        protected BaseQuestion(Recipe recipe)
        {
            Recipe = recipe;
        }

        public List<(int, string)> GetChoices() => Choices.Select((c, i) => (i, c.Text)).ToList();

        public int CountChoices() => Choices.Count;

        public string GetChoice(int index) => index < Choices.Count ? Choices[index].Text : null;

        public List<(Recipe, double?)> EvaluateRecipes(List<Recipe> recipes, int choiceId, string data)
        {
            if (choiceId >= Choices.Count)
            {
                return new List<(Recipe, double?)>();
            }

            var evaluator = Choices[choiceId].Evaluator;
            return recipes.Select(r => (r, evaluator(r, data))).ToList();
        }

        protected void AddChoice(string text, Evaluator evaluator) => Choices.Add(new Choice(text, evaluator));
    }
}