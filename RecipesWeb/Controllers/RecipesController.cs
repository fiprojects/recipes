using Microsoft.AspNetCore.Mvc;
using RecipesCore.Services;

namespace RecipesWeb.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IRecipesService _recipesService;

        public RecipesController(IRecipesService recipesService)
        {
            _recipesService = recipesService;
        }

        public IActionResult Show(long id)
        {
            var recipe = _recipesService.Get(id);
            return View(recipe);
        }
    }
}
