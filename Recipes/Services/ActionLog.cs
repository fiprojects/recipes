using System.Collections.Generic;
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
            string username, string referer, RecommendingAlgorithm algorithm)
        {
            var user = GetUser(username);
            if (user == null)
            {
                return;
            } 

            var record = new ActionLogRecord
            {
                Action = action,
                Recipe = recipe,
                RecommendedRecipe = recommendedRecipe,
                User = user,
                Referer = referer,
                RecommendationAlgorithmIdentifier = algorithm.Identifier
            };
            _db.Add(record);
            _db.SaveChanges();
        }
    }
}