using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipesCore.Services;
using RecipesWeb.SimpleAuth;

namespace RecipesWeb.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IRecipesService _recipesService;
        private readonly Auth _auth;

        public RecipesController(IRecipesService recipesService, Auth auth)
        {
            _recipesService = recipesService;
            _auth = auth;
        }

        public IActionResult Show(long id)
        {
            var recipe = _recipesService.Get(id);
            return View(recipe);
        }
    }
}
