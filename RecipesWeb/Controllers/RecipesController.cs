using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RecipesCore.Models;
using RecipesCore.Services;
using RecipesWeb.ViewModels;

namespace RecipesWeb.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IRecipesService _recipesService;
        private readonly IRatingService _ratingService;

        public RecipesController(IRecipesService recipesService, IRatingService ratingService)
        {
            _ratingService = ratingService;
            _recipesService = recipesService;
        }

        public IActionResult Show(long id)
        {
            double userRatingForRecipe = 0;
            var userName = HttpContext.User.Identity.Name;
            if (userName != null)
            {
                var userRating = _ratingService.GetByUserNameAndRecipeId(userName, id);
                if (userRating != null)
                    userRatingForRecipe = userRating.Rating;
            }

            double averageRating = _ratingService.GetAverageRatingForRecipe(id);
            var viewModel = new RecipesShowModel()
            {
                Recipe = _recipesService.Get(id),
                RecipeUserRating = userRatingForRecipe,
                AverageRating = averageRating
            };

            return View(viewModel);
        }
    }
}
