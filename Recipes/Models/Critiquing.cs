using System;

namespace RecipesCore.Models
{
    public class Critiquing
    {
        public long? Id { get; set; }

        public User User { get; set; }

        public Recipe Recipe { get; set; }

        public double Weight { get; set; }

        public DateTime LastUpdate { get; set; }

        public Critiquing()
        {
        }
    
        public Critiquing(Recipe recipe, double weight)
        {
            Recipe = recipe;
            Weight = weight;
        }
    }
}