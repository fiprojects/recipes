using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public List<UserAllergie> Allergies { get; set; } = new List<UserAllergie>();

        public virtual ICollection<RecipeDone> RecipesDone{ get; set; } = new List<RecipeDone>();

        public virtual ICollection<RecipeSeen> RecipesSeen { get; set; } = new List<RecipeSeen>();

        public virtual ICollection<RecipeRatings> RecipesRatings { get; set; } = new List<RecipeRatings>();

        [InverseProperty("User")]
        public List<FellowCooks> FellowCooks { get; set; } = new List<FellowCooks>();

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
        }
    }
}
