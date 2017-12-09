using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RecipesCore.Info;
using RecipesCore.Services;

namespace RecipesWeb.Controllers
{
    public class SystemController : BaseController
    {
        private readonly IRecipesService _recipesService;
        private readonly ICategoryService _categoryService;
        private readonly IIngredientService _ingredientService;

        public SystemController(IRecipesService recipesService, ICategoryService categoryService, IIngredientService ingredientService)
        {
            _recipesService = recipesService;
            _categoryService = categoryService;
            _ingredientService = ingredientService;
        }

        public IActionResult Info()
        {
            ViewBag.RecipesCount = _recipesService.Count();
            ViewBag.CategoriesCount = _categoryService.Count();
            ViewBag.TopDislikedIngredients = _ingredientService.GetAllByPreference().Take(5);
            ViewBag.Algorithms = RecommendingAlgorithms.GetAll();
            ViewBag.Algorithm = Algorithm;
            return View();
        }

        public IActionResult SwitchAlgorithm(string identifier)
        {
            SetAlgorithm(identifier);
            return RedirectToAction("Info");
        }
    }
}
