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

        public Category Category { get; set; }

        public string Author { get; set; }

        public double Rating { get; set; }

        public TimeSpan PreparationTime { get; set; }

        public TimeSpan CookTime { get; set; }

        public int Servings { get; set; }

        public int Calories { get; set; }

        public List<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();

        public string Directions { get; set; }

        public byte[] Image { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Name: {Name}");
            stringBuilder.AppendLine($"Description: {Description}");
            stringBuilder.AppendLine($"Category: [ {Category} ]");
            stringBuilder.AppendLine($"Author: {Author}");
            stringBuilder.AppendLine($"Rating: {Rating}");
            stringBuilder.AppendLine($"Preparation Time: {PreparationTime.Hours} h {PreparationTime.Minutes} min");
            stringBuilder.AppendLine($"Cook Time: {CookTime.Hours} h {CookTime.Minutes} min");
            stringBuilder.AppendLine($"Servings: {Servings}");
            stringBuilder.AppendLine($"Calories: {Calories} cal");
            stringBuilder.AppendLine($"Directions: {Directions}");

            return stringBuilder.ToString();
        }
    }
}