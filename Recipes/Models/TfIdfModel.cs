using System.Collections.Generic;

namespace RecipesCore.Models
{
    public class TfIdfModel
    {
        public long Id { get; set; }
        
        public Recipe Recipe { get; set; }

        public List<TfIdfElement> Elements { get; set; }
    }
}
