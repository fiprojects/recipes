using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using log4net;
using RecipesCore.Models;

namespace Crawler
{
    public class AllRecipesCrawler : ICrawler
    {
        private const string BaseUrl = "http://allrecipes.com/recipe/";

        public ILog Logger { get; set; }

        public Recipe GetRecipe(long id)
        {
            Logger.Info($"Processing recipe: {id}");
            var source = GetRecipeSource(id);
            var recipeSource = new RecipeSource(source);

            if (!recipeSource.IsValid)
            {
                Logger.Error($"Recipe not found: {id}");
                return null;
            }

            var recipe = new Recipe
            {
                Name = recipeSource.Name,
                Description = recipeSource.Description,
                Categories = recipeSource.Categories,
                Author = recipeSource.Author,
                Rating = recipeSource.Rating,
                PreparationTime = recipeSource.PreparationTime,
                CookTime = recipeSource.CookTime,
                Ingredients = recipeSource.Ingredients,
                Directions = recipeSource.Directions
            };
            
            Logger.Info($"Recipe processed: {recipe}");
            return recipe;
        }

        public List<Recipe> GetRecipes(long firstId, long lastId)
        {
            if (firstId == 0 || lastId == 0)
            {
                throw new ArgumentException("Invalid range of recipe IDs.");
            }

            if (firstId > lastId)
            {
                throw new ArgumentException("Last ID must be larger than first ID.");
            }

            var recipes = new List<Recipe>();
            for (var id = firstId; id <= lastId; id++)
            {
                var recipe = GetRecipe(id);
                if (recipe != null)
                {
                    recipes.Add(recipe);
                }

                Thread.Sleep(3000);
            }
            return recipes;
        }

        private static string GetPage(string url)
        {
            string page;
            var request = WebRequest.Create(url);
            using (var response = request.GetResponse())
            {
                var stream = response.GetResponseStream();
                using (var reader = new StreamReader(stream))
                {
                    page = reader.ReadToEnd();
                }
            }

            return page;
        }

        private static string GetRecipeSource(long id)
            => GetPage(BaseUrl + id);
    }
}
