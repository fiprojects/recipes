using System;
using System.Collections.Generic;
using System.Text;
using RecipesCore.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace RecipesCore.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly RecipesContext _db;

        public IngredientService(RecipesContext db)
        {
            _db = db;
        }

        public List<Ingredient> GetAll()
        {
            return _db.Ingredients.ToList();
        }
        
        public List<long> GetAllIngrediencesIds()
        {
            return _db.Ingredients
                .Select(d => d.Id)
                .Distinct()
                .ToList();
        }

        public int GetIngredienceImportanceById(long id)
        {
            return _db.Ingredients
                .Where(b=> b.Id == id)
                .Select(d => d.Importance)
                .SingleOrDefault();
        }

        public String GetIngredienceNameById(long id)
        {
            return _db.Ingredients.Where(a => a.Id == id)
                .Select(a =>a.Name)
                .SingleOrDefault();
        }

        public List<Ingredient> GetAllByPreference()
        {
            return _db.Ingredients
                .GroupJoin(_db.UserAllergies, i => i.Id, a => a.Ingredient.Id,
                    (i, g) => new { i.Id, i.Name, i.Importance, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ThenBy(g => g.Name)
                .Select(g => new Ingredient { Id = g.Id, Name = g.Name, Importance = g.Importance })
                .ToList();
        }
    }
}
