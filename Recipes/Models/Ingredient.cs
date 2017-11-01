namespace RecipesCore.Models
{
    public class RecipeIngredient
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public Recipe Recipe { get; set; }
    }
}