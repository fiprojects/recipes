using System;
using RecipesCore.Models;
using System.Collections.Generic;

namespace RecipesCore.Services
{
    public interface IIngredientService
    {
        List<Ingredient> GetAll();

        List<long> GetAllIngrediencesIds();

        int GetIngredienceImportanceById(long id);

        String GetIngredienceNameById(long id);

        List<Ingredient> GetAllByPreference();
    }
}
