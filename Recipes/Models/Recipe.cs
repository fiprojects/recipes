using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipesCore.Models
{
    public class Recipe
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public List<string> Categories { get; set; }

        public string Author { get; set; }

        public double Rating { get; set; }

        public TimeSpan PreparationTime { get; set; }

        public TimeSpan CookTime { get; set; }

        [NotMapped]
        public List<string> Ingredients { get; set; } = new List<string>();

        public string Directions { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Name: {Name}");
            stringBuilder.AppendLine($"Description: {Description}");
            stringBuilder.AppendLine($"Categories: [ {(Categories != null ? string.Join(", ", Categories) : string.Empty)} ]");
            stringBuilder.AppendLine($"Author: {Author}");
            stringBuilder.AppendLine($"Rating: {Rating}");
            stringBuilder.AppendLine($"Preparation Time: {PreparationTime.Hours} h {PreparationTime.Minutes} min");
            stringBuilder.AppendLine($"Cook Time: {CookTime.Hours} h {CookTime.Minutes} min");
            stringBuilder.AppendLine($"Ingredients: [ {(Ingredients != null ? string.Join(", ", Ingredients) : string.Empty)} ]");
            stringBuilder.AppendLine($"Directions: {Directions}");

            return stringBuilder.ToString();
        }
    }
}