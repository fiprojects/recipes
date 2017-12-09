using RecipesCore.Models;
using System.Collections.Generic;

namespace RecipesCore.Services
{
    public interface IIngredientService
    {
        List<Ingredient> GetAll();

        List<Ingredient> GetAllByPreference();
    }
}
