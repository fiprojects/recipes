using System;
using System.Collections.Generic;
using System.Text;

namespace RecipesCore
{
    public class Recipe
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> Categories { get; set; }

        public string Author { get; set; }

        public double Rating { get; set; }

        public TimeSpan PreparationTime { get; set; }

        public TimeSpan CookTime { get; set; }
        
        public List<string> Ingredients { get; set; } = new List<string>();

        public string Directions { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Name: {Name}");
            stringBuilder.AppendLine($"Description: {Description}");
            stringBuilder.AppendLine($"Categories: [ {string.Join(", ", Categories)} ]");
            stringBuilder.AppendLine($"Author: {Author}");
            stringBuilder.AppendLine($"Rating: {Rating}");
            stringBuilder.AppendLine($"Preparation Time: {PreparationTime.Hours} h {PreparationTime.Minutes} min");
            stringBuilder.AppendLine($"Cook Time: {CookTime.Hours} h {CookTime.Minutes} min");
            stringBuilder.AppendLine($"Ingredients: [ {string.Join(", ", Ingredients)} ]");
            stringBuilder.AppendLine($"Directions: {Directions}");

            return stringBuilder.ToString();
        }
    }
}