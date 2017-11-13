using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using RecipesCore.Models;
using RecipesCore.Services;
using RecipesWeb.ViewModels;

namespace RecipesWeb.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IRecipesService _recipesService;
        private readonly IRatingService _ratingService;
        private readonly IUserService _userService;

        public RecipesController(IRecipesService recipesService, IRatingService ratingService,
                                IUserService userService)
        {
            _ratingService = ratingService;
            _recipesService = recipesService;
            _userService = userService;
        }

        public IActionResult Show(long id)
        {
            RecipeRatings userRatingForRecipe = new RecipeRatings
            {
                UserId = 0,
                RecipeId = id,
                Rating = 0
            };

            var userName = HttpContext.User.Identity.Name;
            if (userName != null)
            {
                var userId = _userService.Get(userName).Id;
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

            var viewModel = new RecipesShowModel()
            {
                Recipe = _recipesService.Get(id),
                RecipeUserRating = userRatingForRecipe,
                AverageRating = _ratingService.GetAverageRatingForRecipe(id) 
            };

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
    }
}
