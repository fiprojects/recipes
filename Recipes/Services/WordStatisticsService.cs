using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public class WordStatisticsService : IWordStatisticsService
    {
        private readonly RecipesContext _db;

        public WordStatisticsService(RecipesContext db)
        {
            if (db != null)
                _db = db;
        }

        public List<Tuple<string, int>> GetCookingTimeAndFreq()
        {
            List<Tuple<string, int>> cookTimes =  new List<Tuple<string, int>>();
            List<Recipe> recipes = _db.Recipes.Select(a => a).ToList();
            foreach (Recipe r in recipes)
            {
                
            }
            return cookTimes;
        }
    }
}
