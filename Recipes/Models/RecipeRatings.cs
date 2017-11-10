namespace RecipesCore.Models
{
    public class RecipeRatings
    {
        public long UserId { get; set; }

        public User User { get; set; }

        public long RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public int Rating { get; set; }

    }
}
