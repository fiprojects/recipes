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

        public virtual ICollection<RecipeSeen> SeenBy { get; set; } = new List<RecipeSeen>();

        public virtual ICollection<RecipeDone> DoneBy { get; set; } = new List<RecipeDone>();

        public virtual ICollection<RecipeRatings> RecipesRatings { get; set; } = new List<RecipeRatings>();

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

        protected bool Equals(Recipe other)
        {
            return Id == other.Id && string.Equals(Name, other.Name) && string.Equals(Description, other.Description) &&
                   Equals(Category, other.Category) && string.Equals(Author, other.Author) &&
                   Rating.Equals(other.Rating) && PreparationTime.Equals(other.PreparationTime) &&
                   CookTime.Equals(other.CookTime) && Servings == other.Servings && Calories == other.Calories &&
                   string.Equals(Directions, other.Directions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Recipe) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Category != null ? Category.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Author != null ? Author.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Rating.GetHashCode();
                hashCode = (hashCode * 397) ^ PreparationTime.GetHashCode();
                hashCode = (hashCode * 397) ^ CookTime.GetHashCode();
                hashCode = (hashCode * 397) ^ Servings;
                hashCode = (hashCode * 397) ^ Calories;
                hashCode = (hashCode * 397) ^ (Directions != null ? Directions.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}