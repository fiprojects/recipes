namespace RecipesCore.Models
{
    public class FellowCooks
    {
        public long Id { get; set; }

        public User User { get; set; }

        public User FollowedUser { get; set; }
    }
}