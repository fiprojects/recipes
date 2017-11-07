using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipesWeb.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal;
using RecipesCore.Models;
using RecipesCore;

namespace RecipesWeb.Controllers
{
    public class SearchController : Controller
    {
        private readonly RecipesContext db;

        public SearchController(RecipesContext db)
        {
            this.db = db;
        }

        public IActionResult Show()
        {

            var viewModel = new FilterViewModel
            {
                //Ingredients = db.Ingredients.ToList(),
                Selected = new Dictionary<string, bool> { { "egg", false }, { "water", false }, { "floor", false },
                     { "mashrooms", false },  { "oil", false },  { "spenat", false },  { "brokoli", false } },
                Categories = new List<String> { "Pasta", "Soap", "Chichen", "Dezert" },
                Ingred = new List<String> { "egg", "water", "floor", "mashrooms", "oil", "spenat", "brokoli" }
            };
            return View(viewModel);
        }
    }
}