using System.Collections.Generic;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public interface ITfIdfService
    {
        void Add(List<TfIdfModel> models);
        
        void Add(TfIdfModel model);

        List<TfIdfModel> GetAll();

        List<TfIdfModel> GetAllExcept(TfIdfModel model);

        List<Recipe> GetSimilarRecipesForRecipe(Recipe recipe);

        List<Recipe> GetSimilarRecipesForModel(TfIdfModel model);

        List<Recipe> RankListUsingTfIdf(TfIdfModel model, List<TfIdfModel> models);

        double ComputeCosineSimilarity(TfIdfModel a, TfIdfModel b);
    }
}
