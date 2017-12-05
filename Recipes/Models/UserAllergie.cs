using System;
using System.Collections.Generic;

namespace RecipesCore.Models
{
    public class UserAllergie
    {
        public long Id { get; set; }

        public Ingredient Ingredient { get; set; }

        public User User{ get; set; }

        public UserAllergie()
        {
        }

        public UserAllergie(Ingredient ingredient)
        {
            Ingredient = ingredient;
        }
        
        /*public static List<UserAllergie> GetUserAllergiesFromString(string allergies)
        {
            List<UserAllergie> list = new List<UserAllergie>();
            if (allergies == null)
                return list;
            List<string> stringList = new List<string>(allergies.Split(';'));
            foreach (string s in stringList)
            {
                string removedWhiteSpaces = s.Trim();
                if (String.IsNullOrEmpty(removedWhiteSpaces))
                    continue;
                list.Add(new UserAllergie(s));
            }
            return list;
        }*/
    }
}