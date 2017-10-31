using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RecipesCore.Migrations
{
    public partial class Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Ingredients",
                table: "Recipes");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Recipes",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Recipes");

            migrationBuilder.AddColumn<List<string>>(
                name: "Categories",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Ingredients",
                table: "Recipes",
                nullable: true);
        }
    }
}
