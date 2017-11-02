using RecipesCore.Models;

namespace RecipesCore.Services
{
    public interface IUserService
    {
        User Get(string username);

        void Add(User user);

        bool Exists(string username);
    }
}