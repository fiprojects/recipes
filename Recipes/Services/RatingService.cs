using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecipesCore.Models;
using Microsoft.EntityFrameworkCore;

namespace RecipesCore.Services
{
    public class RatingService : IRatingService
    {
        private readonly RecipesContext _db;

        public RatingService(RecipesContext db)
        {
            if(db != null)
                _db = db;
        }

        public List<RecipeRatings> GetAll()
        {
            return _db.RecipeRatings.ToList();
        }

        public List<RecipeRatings> GetByUserId(long userId)
        {
            return _db.RecipeRatings.Where(a => a.UserId == userId).ToList();
        }

        public RecipeRatings GetByUserNameAndRecipeId(string name, long recipeId)
        {
            return _db.RecipeRatings.Where(a => a.User.Username == name)
                .FirstOrDefault(b => b.RecipeId == recipeId);
        }

        public List<RecipeRatings> GetByRecipeId(long recipeId)
        {
            return _db.RecipeRatings.Where(a => a.RecipeId == recipeId).ToList();
        }

        public double GetAverageRatingForUser(long userId)
        {
            List<RecipeRatings> userRatings = GetByUserId(userId);
            return userRatings.Select(a => a.Rating).Sum() / (double)userRatings.Count;
        }

        public double GetAverageRatingForRecipe(long recipeId)
        {
            List<RecipeRatings> recipeRatings = GetByRecipeId(recipeId);
            return recipeRatings.Select(a => a.Rating).Sum() / (double)recipeRatings.Count;
        }

        public void Add(RecipeRatings rating)
        {
            if (rating?.RecipeId == null || rating.UserId == null || rating.Rating > 5 || rating.Rating < 0)
            {
                return;
            }
            if (Exists(rating.RecipeId, rating.UserId ))
            {
                _db.RecipeRatings
                    .Where(s => s.RecipeId == rating.RecipeId)
                    .FirstOrDefault(s => s.UserId == rating.UserId);
            }
            _db.Add(rating);
            _db.SaveChanges();
        }

        public bool Exists(long recipeId, long userId)
        {
            List<RecipeRatings> ratingsForRecipe = GetByRecipeId(recipeId);
            return ratingsForRecipe.Select(a => a.UserId == userId).Any();
        }
    }
}
