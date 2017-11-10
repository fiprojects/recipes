using System;
using System.Collections.Generic;
using System.Text;

namespace RecipesCore.Models
{
    public class RecipeDone
    {
        public long Id { get; set; }

        public Recipe Recipe{ get; set;}

        public User User { get; set; }
    }
}
