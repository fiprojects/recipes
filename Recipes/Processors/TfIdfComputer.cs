using System;
using System.Collections.Generic;
using System.Linq;
using RecipesCore.Models;
using RecipesCore.Services;

namespace RecipesCore.Processors
{
    public class TfIdfComputer : IProcessor
    {
        private readonly IRecipesService _recipesService;
        private readonly ITfIdfService _tfIdfService;

        private readonly Dictionary<Recipe, Dictionary<string, int>> _tfForRecipes = new Dictionary<Recipe, Dictionary<string, int>>();

        private readonly Dictionary<string, int> _termOccurenceInDocs = new Dictionary<string, int>();

        public TfIdfComputer(IRecipesService recipesService, ITfIdfService tfIdfService)
        {
            _recipesService = recipesService;
            _tfIdfService = tfIdfService;
        }

        public void Run(string[] args)
        {
            var recipes = _recipesService.GetAll();
            var models = ComputeTfIdfForRecipes(recipes);
            _tfIdfService.Add(models);
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
                }
                else
                {
                    dict.Add(term, 1);
                    if (_termOccurenceInDocs.ContainsKey(term))
                        _termOccurenceInDocs[term]++;
                    else
                        _termOccurenceInDocs.Add(term, 1);
                }
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
                var tfIdfModel = new TfIdfModel
                {
                    Recipe = recipe
                };
                var termsAndCounts = _tfForRecipes[recipe];
                int maximalFrequency = termsAndCounts.Values.Max();
                foreach (KeyValuePair<string,int> i in termsAndCounts)
                {
                    var element = new TfIdfElement
                    {
                        Term = i.Key,
                        TfIdf = GetTfIdfValue(i.Key, i.Value, maximalFrequency, n)
                    };
                    tfIdfModel.Elements.Add(element);
                }
                models.Add(tfIdfModel);
            }

            return models;
        }

        private double GetTfIdfValue(string term, int termFreq, int maximalFrequency, int n)
        {
            double tf = (double)termFreq / (double)maximalFrequency;
            int nt = _termOccurenceInDocs[term]; // number of documents containing term
            double idf = Math.Log((double) n / (double) nt, 10); // decimal logarithm
            return tf*idf;
        }
    }
}