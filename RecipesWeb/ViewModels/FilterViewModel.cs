using Microsoft.AspNetCore.Mvc.Rendering;
using RecipesCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesWeb.ViewModels
{
    public class FilterViewModel
    {
        public List<RecipeIngredient> Ingredients{ get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public List<Recipe> Recipes { get; set; }
        public List<String> Ingred { get; set; }
        public Dictionary<String, Boolean> Selected { get; set; }
        public int? CategoryId { get; set; }

    }
}
