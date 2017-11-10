using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RecipesCore.Models
{
    public class RecipeDone
    {
        public long UserId { get; set; }

        public User User { get; set; }

        public long RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}
