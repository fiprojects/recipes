using RecipesCore.Models;
using System.Collections.Generic;

namespace RecipesCore.Services
{
    public interface IInredientService
    {
        List<Ingredient> GetAll();
    }
}
