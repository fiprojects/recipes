using System.Collections.Generic;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public interface ITfIdfService
    {
        void Add(List<TfIdfModel> models);
        
        void Add(TfIdfModel model);
    }
}