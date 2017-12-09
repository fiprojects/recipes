using System.Linq;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly RecipesContext _db;

        public CategoryService(RecipesContext db)
        {
            _db = db;
        }

        public Category Get(long id)
        {
            return _db.Categories.FirstOrDefault(x => x.Id == id);
        }

        public int Count()
        {
            return _db.Categories.Count();
        }
    }
}