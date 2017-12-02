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

        List<TfIdfModel> GetSimilarRecipes(TfIdfModel model);
    }
}
