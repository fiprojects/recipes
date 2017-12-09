using System;
using System.Collections.Generic;
using System.Text;
using RecipesCore.Info;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public class ActionLog : IActionLog
    {
        private readonly RecipesContext _db;
        private readonly IUserService _userService;

        public ActionLog(RecipesContext db, IUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public void LogDisplayedRecipe(Recipe recipe, string username, string referer, RecommendingAlgorithm algorithm)
        {
            Log("DisplayRecipe", recipe, null, username, referer, algorithm);
        }

        public void LogRecommendedRecipes(Recipe recipe, List<Recipe> recommendedRecipes, string username, string referer,
            RecommendingAlgorithm algorithm)
        {
            recommendedRecipes.ForEach(r => Log("RecommendedRecipe", recipe, r, username, referer, algorithm));
        }

        public void LogCritiquing(Recipe recipe, string username,
            int questionId, string question, int choiceId, string choice, string data)
        {
            var metadata = new StringBuilder()
                .AppendLine($"questionId: {questionId}")
                .AppendLine($"question: {question}")
                .AppendLine($"choiseId: {choiceId}")
                .AppendLine($"choise: {choice}")
                .AppendLine($"data: {data}")
                .ToString();
            Log("Critiquing", recipe, null, username, null, RecommendingAlgorithms.Get("Critiquing"), metadata);
        }

        private User GetUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            var user = _userService.Get(username);
            return user;
        }

        private void Log(string action, Recipe recipe, Recipe recommendedRecipe,
            string username, string referer, RecommendingAlgorithm algorithm, string metadata = null)
        {
            var user = GetUser(username);
            if (user == null)
            {
                return;
            } 

            var record = new ActionLogRecord
            {
                Date = DateTime.Now,
                Action = action,
                Recipe = recipe,
                RecommendedRecipe = recommendedRecipe,
                User = user,
                Referer = referer,
                RecommendationAlgorithmIdentifier = algorithm.Identifier,
                Metadata = metadata
            };
            _db.Add(record);
            _db.SaveChanges();
        }
    }
}