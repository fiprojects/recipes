using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RecipesCore.Migrations
{
    public partial class RecRecLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RecommendedRecipeId",
                table: "ActionLog",
                type: "int8",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionLog_RecommendedRecipeId",
                table: "ActionLog",
                column: "RecommendedRecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionLog_Recipes_RecommendedRecipeId",
                table: "ActionLog",
                column: "RecommendedRecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionLog_Recipes_RecommendedRecipeId",
                table: "ActionLog");

            migrationBuilder.DropIndex(
                name: "IX_ActionLog_RecommendedRecipeId",
                table: "ActionLog");

            migrationBuilder.DropColumn(
                name: "RecommendedRecipeId",
                table: "ActionLog");
        }
    }
}
