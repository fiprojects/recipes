using System;
using System.Collections.Generic;
using System.Text;
using RecipesCore.Models;
using System.Linq;

namespace RecipesCore.Services
{
    class IngredientService : IInredientService
    {
        private readonly RecipesContext _db;

        public IngredientService(RecipesContext db)
        {
            _db = db;
        }

        public List<Ingredient> GetAll()
        {
            return _db.Ingredients.ToList();
        }
    }
}
