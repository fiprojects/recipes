using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RecipesCore.Critiquing.Questions;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public class CritiquingService : ICritiquingService
    {
        private const double ForgettingFactor = 0.7;

        private readonly RecipesContext _db;
        private readonly IRecipesService _recipesService;
        private readonly IUserService _userService;
        private readonly IActionLog _actionLog;

        public CritiquingService(RecipesContext db, IRecipesService recipesService, IUserService userService,
            IActionLog actionLog)
        {
            _db = db;
            _recipesService = recipesService;
            _userService = userService;
            _actionLog = actionLog;
        }

        public void Critique(string username, long recipeId, int questionId, int choiceId, string data = null)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var user = _userService.Get(username);
            var recipe = _recipesService.Get(recipeId);
            if (user == null || recipe == null)
            {
                return;
            }

            var questions = new Questions(recipe);
            var question = questions[questionId];
            if (question == null)
            {
                return;
            }

            var recipes = _recipesService.GetAll();
            var critiques = _db.Critiquing.Where(c => c.User == user);
            question.EvaluateRecipes(recipes, choiceId, data)
                .ForEach(r =>
                {
                    if (r.Item2 == null)
                    {
                        return;
                    }

                    var critique = critiques.FirstOrDefault(c => Equals(c.Recipe, r.Item1)) 
                        ?? new Models.Critiquing { Recipe = r.Item1, User = user, Weight = 0 };
                    if (Math.Abs(r.Item2.Value) > 1e-4)
                    {
                        var effector = (double) random.Next(50, 100) / 100;
                        critique.Weight = ForgettingFactor * critique.Weight + effector * r.Item2.Value;
                    }
                    critique.LastUpdate = DateTime.Now;

                    if (critique.Id == null)
                    {
                        _db.Add(critique);
                    }
                });

            _actionLog.LogCritiquing(recipe, username, questionId, question.Question,
                choiceId, question.GetChoice(choiceId), data);
            _db.SaveChanges();
        }

        public void Penalize(string username, Recipe recipe, double factor = 1)
        {
            var user = _userService.Get(username);
            if (user == null || recipe == null)
            {
                return;
            }

            var critique = _db.Critiquing.FirstOrDefault(c => c.User == user && Equals(c.Recipe, recipe))
                            ?? new Models.Critiquing { Recipe = recipe, User = user, Weight = 0 };
            if (critique.Weight < 0)
            {
                return;
            }

            critique.Weight -= ForgettingFactor * factor * critique.Weight;
            critique.LastUpdate = DateTime.Now;

            if (critique.Id == null)
            {
                _db.Add(critique);
            }

            _db.SaveChanges();
        }

        public List<Recipe> GetRecommended(int limit, long? userId)
        {
            if (userId == null)
            {
                return _recipesService.GetAll().OrderBy(r => r.Rating).Take(limit).ToList();
            }

            return _db.Recipes
                .Include(r => r.Category)
                .GroupJoin(_db.Critiquing, r => r.Id, c => c.Recipe.Id,
                    (r, cs) => new {r, c = cs.FirstOrDefault(c => c.User.Id == userId)})
                .Select(g => new {Recipe = g.r, Weight = g.c != null ? g.c.Weight : 0})
                .OrderByDescending(g => g.Weight)
                .Select(g => g.Recipe)
                .Take(limit)
                .ToList();
        }
    }
}