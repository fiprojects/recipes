using Microsoft.AspNetCore.Mvc;
using RecipesCore.Models;
using RecipesCore.Services;
using RecipesWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipesWeb.Controllers
{
    public class RecipesController : BaseController
    {
        private readonly IRecipesService _recipesService;
        private readonly IRatingService _ratingService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly ITfIdfService _tfIdfService;

        public RecipesController(IRecipesService recipesService, IRatingService ratingService,
                                IUserService userService, ICategoryService categoryService, ITfIdfService tfIdfService)
        {
            _ratingService = ratingService;
            _recipesService = recipesService;
            _userService = userService;
            _categoryService = categoryService;
            _tfIdfService = tfIdfService;
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
                AverageRating = _ratingService.GetAverageRatingForRecipe(id),
                Recommended = _recipesService.GetRecommendedByIngredience(id, userId)
             };
            
            viewModel.Recommended = GetRecipesByAlgorithm(viewModel.Recipe);
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

        private List<Recipe> GetRecipesByAlgorithm(Recipe currentRecipe)
        {
            switch (Algorithm.Identifier)
            {
                case "Random":
                    return GetRandomRecipes(currentRecipe);
                case "TfIdf":
                    return _tfIdfService.GetSimilarRecipesForRecipe(currentRecipe).Take(5).ToList();
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
    }
}
