using Microsoft.AspNetCore.Mvc;
using RecipesCore.Models;
using RecipesCore.Services;
using RecipesWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipesWeb.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IRecipesService _recipesService;
        private readonly IRatingService _ratingService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;

        public RecipesController(IRecipesService recipesService, IRatingService ratingService,
                                IUserService userService, ICategoryService categoryService)
        {
            _ratingService = ratingService;
            _recipesService = recipesService;
            _userService = userService;
            _categoryService = categoryService;
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

            var viewModel = new RecipesShowModel()
            {
                Recipe = _recipesService.Get(id),
                RecipeUserRating = userRatingForRecipe,
                AverageRating = _ratingService.GetAverageRatingForRecipe(id),
                Recommended = _recipesService.GetRecommendedByIngredience(id, userId)
             };
            //List<Recipe> all = _recipesService.GetRecommendedByCategoryId(viewModel.Recipe.Category.Id).ToList();
            //Random rnd = new Random();
            //List<Recipe> selected = new List<Recipe>();
            //while(selected.Count != 4)
            //{
            //    int index = rnd.Next(all.Count);
            //    if (!selected.Contains(all[index]))
            //    {
            //        selected.Add(all[index]);
            //    }
            //}
            //viewModel.Recommended = selected;

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
    }
}
