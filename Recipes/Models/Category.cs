using System.Collections.Generic;

namespace RecipesCore.Models
{
    public class Category
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long Priority { get; set; }

        public List<Recipe> Recipes { get; set; }

        public Category()
        {
        }

        public Category(string name)
        {
            Name = name;
        }
    }
}