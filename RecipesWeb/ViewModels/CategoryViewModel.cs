using System.Collections.Generic;
using RecipesCore.Models;

namespace RecipesWeb.ViewModels
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }

        public List<Recipe> Recipes { get; set; }
    }
}