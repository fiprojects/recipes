using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RecipesCore.Migrations
{
    public partial class UserAllergiesCB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserAllergies");

            migrationBuilder.AddColumn<long>(
                name: "IngredientId",
                table: "UserAllergies",
                type: "int8",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAllergies_IngredientId",
                table: "UserAllergies",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAllergies_Ingredients_IngredientId",
                table: "UserAllergies",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAllergies_Ingredients_IngredientId",
                table: "UserAllergies");

            migrationBuilder.DropIndex(
                name: "IX_UserAllergies_IngredientId",
                table: "UserAllergies");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "UserAllergies");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserAllergies",
                nullable: true);
        }
    }
}
