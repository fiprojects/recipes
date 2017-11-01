using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public class RecipesService : IRecipesService
    {
        private readonly RecipesContext _db;

        public RecipesService(RecipesContext db)
        {
            _db = db;
        }

        public List<Recipe> GetAll()
        {
            return _db.Recipes.ToList();
        }

        public Recipe Get(long id)
        {
            return _db.Recipes
                .Include(x => x.Ingredients)
                .SingleOrDefault(x => x.Id == id);
        }

        public void Add(Recipe recipe)
        {
            _db.Add(recipe);
            _db.SaveChanges();
        }
    }
}