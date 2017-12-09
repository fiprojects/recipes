namespace RecipesCore.Models
{
    public class ActionLogRecord
    {
        public long? Id { get; set; }

        public string Action { get; set; }

        public Recipe Recipe { get; set; }

        public Recipe RecommendedRecipe { get; set; }

        public User User { get; set; }

        public string Referer { get; set; }

        public string RecommendationAlgorithmIdentifier { get; set; }
    }
}