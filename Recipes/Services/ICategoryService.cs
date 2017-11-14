using RecipesCore.Models;

namespace RecipesCore.Services
{
    public interface ICategoryService
    {
        Category Get(long id);
    }
}
