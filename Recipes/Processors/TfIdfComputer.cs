using System.Collections.Generic;
using RecipesCore.Models;
using RecipesCore.Services;

namespace RecipesCore.Processors
{
    public class TfIdfComputer : IProcessor
    {
        private readonly IRecipesService _recipesService;

        private readonly Dictionary<Recipe, Dictionary<string, int>> _tfForRecipes = new Dictionary<Recipe, Dictionary<string, int>>();

        private readonly Dictionary<string, int> _termOccurenceInDocs = new Dictionary<string, int>();

        public TfIdfComputer(IRecipesService recipesService)
        {
            _recipesService = recipesService;
        }

        public void Run(string[] args)
        {
            
        }
        
        private Dictionary<string, int> GetTermsWithCountForRecipe(Recipe recipe)
        {
            var dict = new Dictionary<string, int>();
            var splitted = recipe.Description.Split(' ');
            foreach (string term in splitted)
            {
                if (dict.ContainsKey(term))
                {
                    dict[term]++;
                    if (_termOccurenceInDocs.ContainsKey(term))
                        _termOccurenceInDocs[term]++;
                    else
                        _termOccurenceInDocs.Add(term, 1);
                }
                else
                    dict.Add(term, 1);
            }
            return dict;
        }

        private List<TfIdfModel> ComputeTfIdfForRecipes(List<Recipe> recipes)
        {
            int n = recipes.Count;
            foreach (Recipe recipe in recipes)
            {
                var dict = GetTermsWithCountForRecipe(recipe);
                _tfForRecipes.Add(recipe, dict);
            }
            
            List<TfIdfModel> models = new List<TfIdfModel>();
            foreach (Recipe recipe in recipes)
            {
                var termsAndCounts = _tfForRecipes[recipe];
                foreach (KeyValuePair<string,int> i in termsAndCounts)
                {
                    var model = new TfIdfModel
                    {
                        Recipe = recipe,
                        Term = i.Key,
                        TfIdf = GetTfIdfValue(termsAndCounts, n)
                    };
                }
            }

            return models;
        }

        private double GetTfIdfValue(Dictionary<string, int> termsAndCounts, int n)
        {
            return 0.0;
        }
    }
}