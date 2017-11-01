using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RecipesCore.Services;
using RecipesWeb.Models;
using RecipesWeb.ViewModels;

namespace RecipesWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRecipesService _recipesService;

        public HomeController(IRecipesService recipesService)
        {
            _recipesService = recipesService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                Recipes = _recipesService.GetAll()
            };

            return View(viewModel);
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
