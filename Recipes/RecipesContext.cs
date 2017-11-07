using Microsoft.EntityFrameworkCore;
using RecipesCore.Models;

namespace RecipesCore
{
    public class RecipesContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<User> Users { get; set; }
        
        public DbSet<UserAllergie> UserAllergies { get; set; }

        public DbSet<RecipeIngredient> Ingredients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new AppConfiguration("DatabaseCredentials.secret.json");
            optionsBuilder.UseNpgsql(config["connectionString"]);
        }
    }
}