using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipesWeb.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal;
using RecipesCore.Models;
using RecipesCore;
using RecipesWeb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipesWeb.Controllers
{
    public class SearchController : Controller
    {
        private readonly RecipesContext db;
        //private FilterViewModel filter;

        public SearchController(RecipesContext db)
        {
            this.db = db;

        }

        public IActionResult Show(FilterViewModel filter)
        {
            if (filter.Categories == null) { 
                var cat = db.Categories.ToList().Select(x =>
                                                    new SelectListItem
                                                    {
                                                        Value = x.Id.ToString(),
                                                        Text = x.Name
                                                    });
            
                filter = new FilterViewModel
                {
                    //Ingredients = db.Ingredients.ToList(),
                    Selected = new Dictionary<string, bool> { { "egg", false }, { "water", false }, { "floor", false },
                         { "mashrooms", false },  { "oil", false },  { "spenat", false },  { "brokoli", false } },
                    Categories = new SelectList(cat, "Value", "Text"),
                    Ingred = new List<String> { "egg", "water", "floor", "mashrooms", "oil", "spenat", "brokoli" },
                    Recipes = db.Recipes.ToList()
                };
            }
            if (filter.CategoryId != null)
            {
                filter.Recipes = db.Recipes.Where(r => r.Category.Id == filter.CategoryId).ToList();
            }
            return View(filter);
        }
    }
}