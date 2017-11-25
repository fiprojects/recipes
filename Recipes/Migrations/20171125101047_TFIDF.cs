using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RecipesCore.Migrations
{
    public partial class TFIDF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TfIdfModels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RecipeId = table.Column<long>(type: "int8", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TfIdfModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TfIdfModels_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TfIdfElement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Term = table.Column<string>(type: "text", nullable: true),
                    TfIdf = table.Column<double>(type: "float8", nullable: false),
                    TfIdfModelId = table.Column<long>(type: "int8", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TfIdfElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TfIdfElement_TfIdfModels_TfIdfModelId",
                        column: x => x.TfIdfModelId,
                        principalTable: "TfIdfModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TfIdfElement_TfIdfModelId",
                table: "TfIdfElement",
                column: "TfIdfModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TfIdfModels_RecipeId",
                table: "TfIdfModels",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TfIdfElement");

            migrationBuilder.DropTable(
                name: "TfIdfModels");
        }
    }
}
