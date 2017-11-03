using Microsoft.AspNetCore.Mvc;
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

        public IActionResult SignUp()
        {
            return Redirect("/");
        }
    }
}