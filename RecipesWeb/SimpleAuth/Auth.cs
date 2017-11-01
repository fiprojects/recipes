using Microsoft.AspNetCore.Http;
using RecipesCore.Models;
using RecipesCore.Services;

namespace RecipesWeb.SimpleAuth
{
    public class Auth
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string CurrentUsername => _httpContextAccessor.HttpContext.Session.GetString("Username");
        public User CurrentUser => _userService.Get(CurrentUsername);
        public bool IsLoggedIn => _userService.Get(CurrentUsername) != null;

        public Auth(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Login(string username)
        {
            if (_userService.Get(username) == null)
            {
                _userService.Add(new User(username));
            }

            _httpContextAccessor.HttpContext.Session.SetString("Username", username);
            return true;
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext.Session.Remove("Username");
        }
    }
}