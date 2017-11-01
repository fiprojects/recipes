using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using log4net;
using RecipesCore.Models;

namespace Crawler
{
    public class AllRecipesCrawler : ICrawler
    {
        private const string BaseUrl = "http://allrecipes.com/recipe/";

        private readonly int _delayBetweenRecipes = 0;

        public AllRecipesCrawler()
        {
        }

        public AllRecipesCrawler(int delayBetweenRecipes)
        {
            _delayBetweenRecipes = delayBetweenRecipes;
        }

        public ILog Logger { get; set; }

        public Recipe GetRecipe(long id)
        {
            Logger.Info($"Processing recipe: {id}");
            var source = GetRecipeSource(id);
            if (source == null)
            {
                Logger.Error($"Recipe not found: {id}");
                return null;
            }

            var recipeSource = new RecipeSource(source);
            if (!recipeSource.IsValid || recipeSource.Name.Contains("Italian Style Chicken Sausage Skillet Pizza"))
            {
                Logger.Error($"Recipe not found: {id}");
                return null;
            }

            var recipe = new Recipe
            {
                Name = recipeSource.Name,
                Description = recipeSource.Description,
                Category = recipeSource.Categories.FirstOrDefault(),
                Author = recipeSource.Author,
                Rating = recipeSource.Rating,
                PreparationTime = recipeSource.PreparationTime,
                CookTime = recipeSource.CookTime,
                Servings = recipeSource.Servings,
                Calories = recipeSource.Calories,
                Ingredients = recipeSource.Ingredients.Select(x => new RecipeIngredient { Name = x }).ToList(),
                Directions = recipeSource.Directions,
                Image = GetImage(recipeSource.ImageUrl)
            };
            
            Logger.Info($"Recipe processed: {recipe}");
            return recipe;
        }

        public void ProcessRecipes(IEnumerable<long> ids, Action<Recipe> processor = null)
        {
            foreach (var id in ids)
            {
                var recipe = GetRecipe(id);
                if (recipe != null)
                {
                    processor?.Invoke(recipe);
                }

                Thread.Sleep(_delayBetweenRecipes);
            }
        }

        public List<Recipe> GetRecipes(IEnumerable<long> ids)
        {
            var recipes = new List<Recipe>();
            ProcessRecipes(ids, recipes.Add);
            return recipes;
        }

        private static string GetPage(string url)
        {
            string page;
            try
            {
                ServicePointManager.Expect100Continue = false;
                var request = WebRequest.Create(url);
                request.Proxy = null;

                using (var response = request.GetResponse())
                {
                    var stream = response.GetResponseStream();
                    using (var reader = new StreamReader(stream))
                    {
                        page = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                page = null;
            }

            return page;
        }

        private static string GetRecipeSource(long id)
            => GetPage(BaseUrl + id);

        private static byte[] GetImage(string url)
        {
            if (url == null)
            {
                return null;
            }

            var client = new WebClient();
            try
            {
                return client.DownloadData(url);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
