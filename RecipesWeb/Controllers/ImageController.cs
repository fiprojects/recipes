using Microsoft.AspNetCore.Mvc;
using RecipesCore.Services;

namespace RecipesWeb.Controllers
{
    public class ImageController : Controller
    {
        private readonly IRecipesService _recipesService;

        public ImageController(IRecipesService recipesService)
        {
            _recipesService = recipesService;
        }

        public IActionResult Get(long id)
        {
            var recipe = _recipesService.Get(id);
            if (recipe == null)
            {
                return NotFound();
            }

            return File(recipe.Image, "image/jpeg");
        }
    }
}