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
                .Include(x => x.Ingredients).ThenInclude(i => i.Ingredient)
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
                .Include(x => x.Ingredients).ThenInclude(i => i.Ingredient)
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
                .OrderByDescending(r => r.Rating)
                .ToList();
        }

        public List<Recipe> GetRecommendedByCategoryId(long id)
        {
            return _db.Recipes
                .Where(x => x.Category.Id == id)
                .Include(x => x.Category)
                .Include(x => x.Ingredients)
                .OrderBy(r => r.Rating)
                .ToList();
        }

        public List<Recipe> GetRecommendedByIngredience(long recipeId)
        {
            Recipe recipe = Get(recipeId);
            List<Recipe> all = GetAll();
            Dictionary<Recipe, int> sameIngredientsCount = new Dictionary<Recipe, int>();
            foreach (Recipe r in all)
            {
                int count = 0;
                foreach (RecipeIngredient i in recipe.Ingredients)
                {   
                    foreach(RecipeIngredient ri in r.Ingredients)
                    {
                        if (ri.Ingredient != null && i.Ingredient != null)
                        {
                            if (ri.Ingredient.Id.Equals(i.Ingredient.Id))
                            {
                                count++;
                            }
                        }
                    }
                }
                sameIngredientsCount.Add(r, count);
            }
            var toRecommend = sameIngredientsCount.ToList();
            toRecommend.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            return toRecommend.Select(kvp => kvp.Key).Take(4).ToList();
        }
    }
}