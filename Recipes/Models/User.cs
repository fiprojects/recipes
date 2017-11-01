using System.ComponentModel.DataAnnotations;

namespace RecipesCore.Models
{
    public class User
    {
        public long? Id { get; set; }

        [Required]
        [MinLength(2)]
        public string Username { get; set; }

        public User()
        {
        }

        public User(string username)
        {
            Username = username;
        }
    }
}