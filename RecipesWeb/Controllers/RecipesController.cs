using Microsoft.AspNetCore.Mvc;
using RecipesCore.Models;
using RecipesCore.Services;
using RecipesWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using RecipesCore.Critiquing.Questions;

namespace RecipesWeb.Controllers
{
    public class RecipesController : BaseController
    {
        private readonly IRecipesService _recipesService;
        private readonly IRatingService _ratingService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly ITfIdfService _tfIdfService;
        private readonly IActionLog _actionLog;
        private readonly ICritiquingService _critiquingService;

        public RecipesController(IRecipesService recipesService, IRatingService ratingService,
                                IUserService userService, ICategoryService categoryService, ITfIdfService tfIdfService,
                                IActionLog actionLog, ICritiquingService critiquingService)
        {
            _ratingService = ratingService;
            _recipesService = recipesService;
            _userService = userService;
            _categoryService = categoryService;
            _tfIdfService = tfIdfService;
            _actionLog = actionLog;
            _critiquingService = critiquingService;
        }

        public IActionResult Show(long id)
        {
            RecipeRatings userRatingForRecipe = new RecipeRatings
            {
                UserId = 0,
                RecipeId = id,
                Rating = 0
            };

            long? userId = null;
            var userName = HttpContext.User.Identity.Name;
            if (userName != null)
            {
                userId = _userService.Get(userName).Id;
                if (userId != null)
                {
                    userRatingForRecipe.UserId = userId.Value;
                }
                var userRating = _ratingService.GetByUserNameAndRecipeId(userName, id);
                if (userRating != null)
                {
                    userRatingForRecipe.Rating = userRating.Rating;
                }
            }

            var viewModel = new RecipesShowModel
            {
                Recipe = _recipesService.Get(id),
                RecipeUserRating = userRatingForRecipe,
                AverageRating = _ratingService.GetAverageRatingForRecipe(id)
             };

            // Critiquing
            if (userId != null && Algorithm.Identifier == "Critiquing")
            {  
                _critiquingService.Penalize(userName, viewModel.Recipe);
                PrepareCritiquing(viewModel);
            }

            viewModel.Recommended = GetRecipesByAlgorithm(viewModel.Recipe, userId);
            PenalizeRecommended(userName, viewModel.Recommended);

            // Log
            var referer = Request.Headers["Referer"].ToString();
            _actionLog.LogDisplayedRecipe(viewModel.Recipe, userName, referer, Algorithm);
            _actionLog.LogRecommendedRecipes(viewModel.Recipe, viewModel.Recommended, userName, referer, Algorithm);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Show(RecipesShowModel model)
        {
            if (ModelState.IsValid)
            {
                _ratingService.Add(model.RecipeUserRating);
            }

            return Show(model.RecipeUserRating.RecipeId);
        }

        public IActionResult Category(long id)
        {
            var viewModel = new CategoryViewModel
            {
                Category = _categoryService.Get(id),
                Recipes = _recipesService.GetAllByCategoryId(id)
            };
            return View(viewModel);
        }

        public IActionResult Critique(long recipeId, int question, int choice, string data = null)
        {
            _critiquingService.Critique(HttpContext.User.Identity.Name, recipeId, question,
                choice, data);
            return RedirectToAction("Show", new { Id = recipeId });
        }

        private void PrepareCritiquing(RecipesShowModel viewModel)
        {
            if (Algorithm.Identifier != "Critiquing")
            {
                return;
            }

            var questions = new Questions(viewModel.Recipe);
            var question = questions.RandomQuestion();
            viewModel.CritiquingQuestionIndex = question.Item1;
            viewModel.CritiquingQuestion = question.Item2;
        }

        private List<Recipe> GetRecipesByAlgorithm(Recipe currentRecipe, long? userId)
        {
            switch (Algorithm.Identifier)
            {
                case "Random":
                    return GetRandomRecipes(currentRecipe);
                case "Ingredients":
                    return _recipesService.GetRecommendedByIngredience(currentRecipe.Id, userId);
                case "TfIdf":
                    return _tfIdfService.GetSimilarRecipesForRecipe(currentRecipe).Take(5).ToList();
                case "Critiquing":
                    return _critiquingService.GetRecommended(5, userId);
                default:
                    return new List<Recipe>();
            }
        }

        private List<Recipe> GetRandomRecipes(Recipe currentRecipe)
        {
            var all = _recipesService.GetRecommendedByCategoryId(currentRecipe.Category.Id).ToList();
            var rnd = new Random();
            var selected = new List<Recipe>();
            while (selected.Count != 5)
            {
                var index = rnd.Next(all.Count);
                if (!selected.Contains(all[index]))
                {
                    selected.Add(all[index]);
                }
            }
            return selected;
        }

        private void PenalizeRecommended(string username, List<Recipe> recipes)
        {
            if (string.IsNullOrWhiteSpace(username) || Algorithm.Identifier != "Critiquing")
            {
                return;
            }

            recipes.ForEach(r => _critiquingService.Penalize(username, r, 0.1));
        }
    }
}
