using Microsoft.AspNetCore.Mvc.Rendering;
using RecipesCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesWeb.ViewModels
{
    public class RegisterViewModel
    {
        public List<Ingredient> Ingredients{ get; set; }

    }
}
