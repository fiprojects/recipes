using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RecipesCore.Models;
using RecipesCore.Services;

namespace RecipesWeb.Controllers
{
    public class SignUpController : Controller
    {
        private readonly IUserService _userService;

        public SignUpController(IUserService userService)
        {
            _userService = userService;
        }
        
        public IActionResult Show()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string user_login, string vegetarian, string vegan, string allergies)
        {
            var username = user_login;
            if (string.IsNullOrWhiteSpace(username) || _userService.Exists(user_login))
            {
                return Redirect("/Error");
            }
            
            bool bVegetarian = vegetarian?.Equals("on") ?? false;
            bool bVegan = vegan?.Equals("on") ?? false;
            List<UserAllergie> allergiesList = UserAllergie.GetUserAllergiesFromString(allergies);
            User user = new User(username, bVegetarian, bVegan, allergiesList);
            _userService.Add(user);
            return Redirect("/");
        }

    }
}
