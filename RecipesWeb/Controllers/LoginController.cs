using Microsoft.AspNetCore.Mvc;

namespace RecipesWeb.Controllers
{
    public class LoginController : Controller
    {
        public LoginController()
        {
            
        }

        public IActionResult Show()
        {
            return View();
        }
        
    }
}