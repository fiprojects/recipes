using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public class TfIdfService : ITfIdfService
    {
        private readonly RecipesContext _db;

        public TfIdfService(RecipesContext db)
        {
            _db = db;
        }

        public void Add(List<TfIdfModel> models)
        {
            foreach (var tfIdfModel in models)
            {
                Add(tfIdfModel);
            }
        }

        public void Add(TfIdfModel model)
        {
            if (model == null)
                return;
            _db.Add(model);
            _db.SaveChanges();
        }

        public List<TfIdfModel> GetAll()
        {
            return _db.TfIdfModels
                .Include(m => m.Recipe)
                .Include(m => m.Recipe.Category)
                .Include(m => m.Elements)
                .ToList();
        }

        public List<TfIdfModel> GetAllExcept(TfIdfModel model)
        {
            var list = GetAll();
            return list.Where(x => !x.Equals(model)).ToList();
        }

        public List<Recipe> GetSimilarRecipesForRecipe(Recipe recipe)
        {
            var model = _db.TfIdfModels
                .Include(m => m.Elements)
                .SingleOrDefault(m => m.Recipe.Equals(recipe));
            return GetSimilarRecipesForModel(model);
        }
        
        public List<Recipe> GetSimilarRecipesForModel(TfIdfModel model)
        {
            var models = GetAllExcept(model);
            var sortedModels = RankListUsingTfIdf(model, models);
            return sortedModels;
        }

        public List<Recipe> RankListUsingTfIdf(TfIdfModel model, List<TfIdfModel> models)
        {
            var tuplesList = new List<Tuple<Recipe, double>>();
            foreach (var tfIdfModel in models)
            {
                double similarity = ComputeCosineSimilarity(model, tfIdfModel);
                tuplesList.Add(Tuple.Create(tfIdfModel.Recipe, similarity));
            }
            var ret = tuplesList.OrderByDescending(tuple => tuple.Item2)
                .Select(tuple => tuple.Item1)
                .ToList();
            return ret;
        }

        public double ComputeCosineSimilarity(TfIdfModel a, TfIdfModel b)
        {
            double threshold = 1e-16;
            double dotProduct = 0.0;
            double sizeA = 0.0;
            double sizeB = 0.0;
            foreach (var element in a.Elements)
            {
                var retElement = b.Elements.Find(e => e.Term.Equals(element.Term));
                if (retElement == null)
                {
                    sizeA += element.TfIdf * element.TfIdf;
                    continue;
                }
                dotProduct += element.TfIdf * retElement.TfIdf;
                sizeA += element.TfIdf * element.TfIdf;
                sizeB += retElement.TfIdf * retElement.TfIdf;
            }
            var diffList = b.Elements.Where(i => a.Elements.All(ai => ai.Term != i.Term)).ToList();
            foreach (var element in diffList)
            {
                sizeB += element.TfIdf * element.TfIdf;
            }
            if (sizeA < threshold || sizeB < threshold)
                return -1.0;
            return dotProduct / (Math.Sqrt(sizeA) * Math.Sqrt(sizeB));
        }

        public List<Tuple<string, int>> GetNumberOfRecipesWhereUsedForTerms()
        {
            return _db.TfIdfModels
                .Include(m => m.Elements)
                .SelectMany(a => a.Elements)
                .GroupBy(b => b.Term)
                .Select(group => Tuple.Create(group.Key, group.Count()))
                .OrderByDescending(c => c.Item2)
                .ToList();
        }

        public List<Tuple<int, int>> GetSumOfTermsUsedInTheSameNumberOfRecipes()
        {
            List<Tuple<string, int>> termnAndCountOfRecipes = GetNumberOfRecipesWhereUsedForTerms();
            return termnAndCountOfRecipes.GroupBy(a => a.Item2)
                .Select(group => Tuple.Create(group.Key, group.Count()))
                .OrderBy(c => c.Item1)
                .ToList();
        }
    }
}
