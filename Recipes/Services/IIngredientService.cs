using System;
using RecipesCore.Models;
using System.Collections.Generic;

namespace RecipesCore.Services
{
    public interface IIngredientService
    {
        List<Ingredient> GetAll();

        List<long> GetAllIngredienceIds();

        String GetIngredienceNameById(long id);

        List<Ingredient> GetAllByPreference();
    }
}
