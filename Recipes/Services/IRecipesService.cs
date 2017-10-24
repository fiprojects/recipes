using System.Collections.Generic;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public interface IRecipesService
    {
        List<Recipe> GetAll();

        Recipe Get(long id);

        void Add(Recipe recipe);
    }
}