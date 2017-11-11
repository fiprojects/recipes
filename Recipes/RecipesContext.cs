using Microsoft.EntityFrameworkCore;
using RecipesCore.Models;

namespace RecipesCore
{
    public class RecipesContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<User> Users { get; set; }
        
        public DbSet<UserAllergie> UserAllergies { get; set; }

        public DbSet<RecipeRatings> RecipeRatings { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeDone>()
                .HasKey(a => new { a.RecipeId, a.UserId });

            modelBuilder.Entity<RecipeSeen>()
                .HasKey(a => new {a.RecipeId, a.UserId});

            modelBuilder.Entity<RecipeRatings>()
                .HasKey(a => new { a.RecipeId, a.UserId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new AppConfiguration("DatabaseCredentials.secret.json");
            optionsBuilder.UseNpgsql(config["connectionString"]);
        }

        
    }
}