using Microsoft.EntityFrameworkCore;
using RecipesCore.Models;

namespace RecipesCore
{
    public class RecipesContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new AppConfiguration("DatabaseCredentials.secret.json");
            optionsBuilder.UseNpgsql(config["connectionString"]);
        }
    }
}