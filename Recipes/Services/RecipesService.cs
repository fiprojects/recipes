using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RecipesCore.Models;
using System;
using Remotion.Linq.Clauses.ResultOperators;

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

        public int Count()
        {
            return _db.Recipes.Count();
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

        public List<Recipe> GetRecommendedByIngredience(long recipeId, long? userId)
        {
           
            Recipe recipe = Get(recipeId);
            List<Recipe> all = new List<Recipe>();
            all = userId != null ? GetRecipesForUser(userId) : GetAll();
            Dictionary<Recipe, int> sameIngredientsCount = new Dictionary<Recipe, int>();
            foreach (Recipe r in all)
            {
                int count = 0;
                if ( r.Id != recipe.Id) { 
                    foreach (RecipeIngredient i in recipe.Ingredients)
                    {   
                        foreach(RecipeIngredient ri in r.Ingredients)
                        {
                            if (ri.Ingredient != null && i.Ingredient != null)
                            {
                                if (ri.Ingredient.Id.Equals(i.Ingredient.Id))
                                {
                                    count = count + i.Ingredient.Importance;
                                }
                            }
                        }
                    }
                    sameIngredientsCount.Add(r, count);
                }
            }
            var toRecommend = sameIngredientsCount.ToList();
            toRecommend.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            return toRecommend.Select(kvp => kvp.Key).Take(5).ToList();
        }

        public List<Recipe> GetMRecipesFromNBestRated(int m = 12, int n = 50)
        {
            if (m > n)
                throw new ArgumentException("m is greater than n (" + m + " > " + n + ")");
            var list = GetTopRecommended();
            var topN = list.Where(r => r.Image != null).Take(n).ToList();
            if (topN.Count < m)
                throw new ArgumentException("Not enough data. Want " + m + ", only " + list.Count + " available with image");
            Random rnd = new Random();
            List<Recipe> selected = new List<Recipe>();
            while(selected.Count != m)
            {
                int index = rnd.Next(n);
                if (!selected.Contains(topN[index]))
                {
                    selected.Add(topN[index]);
                }
            }
            return selected.OrderByDescending(r => r.Rating).ToList();
        }

        public List<double> GetOrderedAllDownloadedRatings()
        {
            List<double> ratings = GetAll().Select(a => a.Rating).OrderByDescending(a => a).ToList();
            return ratings;
        }

        public List<Recipe> GetRecipesForUser(long? userId)
        {
            List<long> ingredientsIds = _db.UserAllergies.Where(x => x.User.Id == userId).Select(x => x.Ingredient.Id).ToList();

            return _db.Recipes
                .Include(x => x.Category)
                .Include(x => x.Ingredients).ThenInclude(i => i.Ingredient)
                .Where(x => !x.Ingredients.Any(i => ingredientsIds.Contains(i.Ingredient.Id)))
                .ToList();

        }

        public List<Recipe> GetRecipesWithIngredienceById(long id)
        {
            return _db.Recipes
                .Include(b => b.Ingredients)
                .ThenInclude(g => g.Ingredient)
                .SelectMany(c => c.Ingredients)
                .Where(d => d.Ingredient != null)
                .Where(e => e.Ingredient.Id == id)
                .Select(f => f.Recipe)
                .ToList();
        }

        public double GetNumberOfRecipesWithIngredienceById(long id)
        {
            return GetRecipesWithIngredienceById(id).Count;
        }

        public List<Tuple<int, int>> GetCookingTimesAndRecipesCount()
        {
            return _db.Recipes
                .GroupBy(a => a.CookTime.Minutes)
                .Select(group => Tuple.Create(group.Key, group.Count()))
                .OrderBy(c => c.Item1)
                .ToList();
        }

        public List<Tuple<int, int>> GetPreparationTimesAndRecipesCount()
        {
            return _db.Recipes
                .GroupBy(a => a.PreparationTime.Minutes)
                .Select(group => Tuple.Create(group.Key, group.Count()))
                .OrderBy(c => c.Item1)
                .ToList();
        }

        public List<Tuple<int, int>> GetCookAndPrepTimesAndRecipesCount()
        {
            return _db.Recipes
                .GroupBy(a => a.PreparationTime.Minutes + a.CookTime.Minutes)
                .Select(group => Tuple.Create(group.Key, group.Count()))
                .OrderBy(c => c.Item1)
                .ToList();
        }

        public List<Tuple<string, int>> GetCountOfRecipesInCategory()
        {
            return _db.Recipes
                .Include(a => a.Category)
                .Where(b => b.Category!= null)
                .Select(c => Tuple.Create(c.Category.Name, c.Category.Recipes.Count))
                .Distinct()
                .ToList();
        }
    }
}