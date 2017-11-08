namespace RecipesCore.Models
{
    public class RecipeIngredient
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public Recipe Recipe { get; set; }

        public string Quantity { get; set; }

        public string Measure { get; set; }

        public Ingredient Ingredient { get; set; }
    }
}