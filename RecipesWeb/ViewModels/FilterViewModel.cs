using RecipesCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesWeb.Models
{
    public class FilterViewModel
    {
        public List<RecipeIngredient> Ingredients{ get; set; }

        public List<String> Categories { get; set; }
        public List<String> Ingred { get; set; }
        public Dictionary<String, Boolean> Selected { get; set; }

    }
}
