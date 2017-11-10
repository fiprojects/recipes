using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesCore;

namespace RecipesWeb.ViewComponents
{
    public class CategoryMenu : ViewComponent
    {
        private readonly RecipesContext _db;

        public CategoryMenu(RecipesContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _db.Categories.OrderByDescending(c => c.Priority).ToListAsync();
            return View(categories);
        }
    }
}