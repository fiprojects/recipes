using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RecipesCore.Models;
using RecipesCore.Services;
using RecipesWeb.ViewModels;

namespace RecipesWeb.Controllers
{
    public class SignUpController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IIngredientService _ingredientService;

        public SignUpController(IUserService userService, IIngredientService ingredientService)
        {
            _userService = userService;
            _ingredientService = ingredientService;
        }
        
        public IActionResult Show()
        {
            return View(PrepareViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string userLogin, string vegetarian, string vegan, string allergies,
            [Bind] RegisterViewModel registerViewModel)
        {
            var ingredients = _ingredientService.GetAll();
            var username = userLogin;
            if (string.IsNullOrWhiteSpace(username) || _userService.Exists(userLogin))
            {
                return Redirect("/Error");
            }
            
            bool bVegetarian = vegetarian?.Equals("on") ?? false;
            bool bVegan = vegan?.Equals("on") ?? false;
            var allergiesList = registerViewModel.Ingredients
                .Where(i => i.Selected)
                .Select(i => ingredients.First(j => j.Id == i.Id))
                .Select(i => new UserAllergie(i))
                .ToList();
            User user = new User(username, bVegetarian, bVegan, allergiesList);
            _userService.Add(user);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User")
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            
            return Redirect("/");
        }

        private RegisterViewModel PrepareViewModel()
        {
            var viewModel = new RegisterViewModel();
            _ingredientService.GetAllByPreference().ForEach(i =>
            {
                viewModel.Ingredients.Add(new SelectedItemViewModel { Id = i.Id, Name = i.Name });
            });

            return viewModel;
        }
    }
}
