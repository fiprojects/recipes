using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RecipesCore.Migrations
{
    public partial class RecipeSeen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipeSeen",
                columns: table => new
                {
                    RecipeId = table.Column<long>(type: "int8", nullable: false),
                    UserId = table.Column<long>(type: "int8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSeen", x => new { x.RecipeId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RecipeSeen_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeSeen_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSeen_UserId",
                table: "RecipeSeen",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeSeen");
        }
    }
}
