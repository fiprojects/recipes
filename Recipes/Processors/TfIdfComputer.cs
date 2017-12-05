using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RecipesCore.Models;
using RecipesCore.Services;

namespace RecipesCore.Processors
{
    public class TfIdfComputer : IProcessor
    {
        private readonly IRecipesService _recipesService;
        private readonly ITfIdfService _tfIdfService;

        private readonly Dictionary<Recipe, Dictionary<string, int>> _tfForRecipes = 
            new Dictionary<Recipe, Dictionary<string, int>>();

        private readonly Dictionary<string, int> _termOccurenceInDocs = new Dictionary<string, int>();

        private readonly char[] _charsToRemove =  {',', '.', ';', ':', '?', '!', '(', ')', '{', '}', '[', ']', '<',
            '>', '\'', '"', '-'};

        private StopWords _stopWords;

        public TfIdfComputer(IRecipesService recipesService, ITfIdfService tfIdfService, string stopWordsFile)
        {
            _recipesService = recipesService;
            _tfIdfService = tfIdfService;
            _stopWords = new StopWords(stopWordsFile);
        }

        public void Run(string[] args)
        {
            var recipes = _recipesService.GetAll();
            var models = ComputeTfIdfForRecipes(recipes);
            _tfIdfService.Add(models);
        }
        
        public Dictionary<string, int> GetTermsWithCountForRecipe(Recipe recipe)
        {
            var dict = new Dictionary<string, int>();
            var replaced = Regex.Replace(recipe.Directions, @"\r\n?|\n", " ");
            var splitted = replaced.Split(' ');
            foreach (string term in splitted)
            {
                string normalized = NormalizeTerm(term);
                if (normalized.Length == 0)
                    continue;
                if (dict.ContainsKey(normalized))
                {
                    dict[normalized]++;
                }
                else
                {
                    dict.Add(normalized, 1);
                    if (_termOccurenceInDocs.ContainsKey(normalized))
                        _termOccurenceInDocs[normalized]++;
                    else
                        _termOccurenceInDocs.Add(normalized, 1);
                }
            }
            return dict;
        }

        public List<TfIdfModel> ComputeTfIdfForRecipes(List<Recipe> recipes)
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
                if (termsAndCounts.Count == 0)
                    continue;
                var tfIdfModel = new TfIdfModel
                {
                    Recipe = recipe
                };
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

        public double GetTfIdfValue(string term, int termFreq, int maximalFrequency, int n)
        {
            double tf = (double)termFreq / (double)maximalFrequency;
            int nt = _termOccurenceInDocs[term]; // number of documents containing term
            double idf = Math.Log((double) n / (double) nt, 10); // decimal logarithm
            return tf*idf;
        }

        public string NormalizeTerm(string term)
        {
            string ret = term.TrimStart(_charsToRemove).TrimEnd(_charsToRemove);
            ret = ret.ToLower();
            if (_stopWords.IsStopWord(ret))
                return "";
            int n;
            if (int.TryParse(ret, out n))
                return "";
            return ret;
        }
    }
}