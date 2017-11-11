using System.Text;

namespace RecipesCore.Models
{
    public class RecipeRatings
    {
        public long UserId { get; set; }

        public User User { get; set; }

        public long RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public int Rating { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"User: [ {User} ]");
            stringBuilder.AppendLine($"Recipe: [ {Recipe} ]");
            stringBuilder.AppendLine($"Rating: {Rating}");

            return stringBuilder.ToString();
        }
    }
}
