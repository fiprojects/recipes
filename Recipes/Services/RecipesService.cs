using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RecipesCore.Models;
using System;

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
            return _db.Recipes
                .Include(x => x.Category)
                .ToList();
        }

        public List<Recipe> GetAllByCategoryId(long id)
        {
            return _db.Recipes
                .Where(x => x.Category.Id == id)
                .Include(x => x.Category)
                .Include(x => x.Ingredients)
                .ToList();
        }

        public Recipe Get(long id)
        {
            return _db.Recipes
                .Include(x => x.Category)
                .Include(x => x.Ingredients)
                .SingleOrDefault(x => x.Id == id);
        }

        public void Add(Recipe recipe)
        {
            var category = _db.Categories.SingleOrDefault(c => c.Name == recipe.Category.Name);
            if (category == null)
            {
                category = recipe.Category;
                _db.Categories.Add(category);
                _db.SaveChanges();
            }

            recipe.Category = category;
            _db.Add(recipe);
            _db.SaveChanges();
        }

        public List<Recipe> GetTopRecommended()
        {
            return _db.Recipes
                .Include(x => x.Category)
                .Include(x => x.Ingredients)
                .OrderBy(r => r.Rating)
                .ToList();
        }

        public List<Recipe> GetRecommendedByCategoryId(long id)
        {
            List<Recipe> all = _db.Recipes
                .Where(x => x.Category.Id == id)
                .Include(x => x.Category)
                .Include(x => x.Ingredients)
                .OrderBy(r => r.Rating)
                .ToList();
            Random rnd = new Random();
            List<Recipe> result = new List<Recipe>();
            for (int i = 0; i < 4; i++)
            {
                int index = rnd.Next(0, all.Count);
                result.Add(all[index]);
            }

            return result;
        }


    }
}