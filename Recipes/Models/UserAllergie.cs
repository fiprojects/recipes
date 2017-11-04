using System.Collections.Generic;

namespace RecipesCore.Models
{
    public class UserAllergie
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public User User{ get; set; }

        public UserAllergie()
        {
            
        }

        public UserAllergie(string name)
        {
            Name = name;
        }
        
        public static List<UserAllergie> GetUserAllergiesFromString(string alergies)
        {
            List<UserAllergie> list = new List<UserAllergie>();
            List<string> stringList = new List<string>(alergies.Split(';'));
            foreach (string s in stringList)
            {
                string removedWhiteSpaces = s.Trim();
                list.Add(new UserAllergie(s));
            }
            return list;
        }
    }
}