using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RecipesCore.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Author = table.Column<string>(type: "text", nullable: true),
                    Categories = table.Column<List<string>>(type: "text[]", nullable: true),
                    CookTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Directions = table.Column<string>(type: "text", nullable: true),
                    Ingredients = table.Column<List<string>>(type: "text[]", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    PreparationTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Rating = table.Column<double>(type: "float8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
