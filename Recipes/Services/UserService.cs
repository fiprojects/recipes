using System.Linq;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public class UserService : IUserService
    {
        private readonly RecipesContext _db;

        public UserService(RecipesContext db)
        {
            _db = db;
        }

        public User Get(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            return _db.Users.SingleOrDefault(u => u.Username == username);
        }

        public void Add(User user)
        {
            if (user?.Username == null || user.Username.Length < 3)
            {
                return;
            }

            _db.Add(user);
            _db.SaveChanges();
        }
    }
}