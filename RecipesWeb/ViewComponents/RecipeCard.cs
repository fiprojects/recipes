using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipesCore.Models;

namespace RecipesWeb.ViewComponents
{
    public class RecipeCard : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Recipe recipe, bool displayCategory = false)
        {
            ViewBag.DisplayCategory = displayCategory;
            return await Task.FromResult<IViewComponentResult>(View(recipe));
        }
    }
}