using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipesCore.Models
{
    public class User
    {
        public long? Id { get; set; }

        [Required]
        [MinLength(2)]
        public string Username { get; set; }
        
        public bool Vegetarian { get; set; }
        
        public bool Vegan { get; set; }

        private List<UserAllergie> Allergies { get; set; } = new List<UserAllergie>();

        public User()
        {
        }

        public User(string username) : this(username, false, false, new List<UserAllergie>())
        {
        }

        public User(string username, bool vegetarian, bool vegan, List<UserAllergie> allergies)
        {
            Username = username;
            Vegetarian = vegetarian;
            Vegan = vegan;
            Allergies = allergies;
            foreach (var allergy in Allergies)
            {
                allergy.User = this;
            }
        }
    }
}