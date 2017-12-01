using System.Collections.Generic;
using System.Linq;
using RecipesCore.Migrations;
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
            return _db.TfIdfModels.ToList();
        }

        public List<TfIdfModel> GetAllExcept(TfIdfModel model)
        {
            var list = GetAll();
            return list.Where(x => !x.Equals(model)).ToList();
        }
        
        public List<TfIdfModel> GetSimilarRecipes(TfIdfModel model)
        {
            return null;
        }
    }
}
