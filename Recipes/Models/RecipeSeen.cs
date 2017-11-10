using System;
using System.Collections.Generic;
using System.Text;

namespace RecipesCore.Models
{
    public class RecipeSeen
    {
        public long Id { get; set; }

        public User User { get; set; }

        public Recipe Recipe { get; set; }
    }
}
