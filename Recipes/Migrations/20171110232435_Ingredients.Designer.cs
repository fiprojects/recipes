﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RecipesCore;
using System;

namespace RecipesCore.Migrations
{
    [DbContext(typeof(RecipesContext))]
    [Migration("20171110232435_Ingredients")]
    partial class Ingredients
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("RecipesCore.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<long>("Priority");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("RecipesCore.Models.FellowCooks", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("FollowedUserId");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("FollowedUserId");

                    b.HasIndex("UserId");

                    b.ToTable("FellowCooks");
                });

            modelBuilder.Entity("RecipesCore.Models.Ingredient", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("RecipesCore.Models.Recipe", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<int>("Calories");

                    b.Property<long?>("CategoryId");

                    b.Property<TimeSpan>("CookTime");

                    b.Property<string>("Description");

                    b.Property<string>("Directions");

                    b.Property<byte[]>("Image");

                    b.Property<string>("Name");

                    b.Property<TimeSpan>("PreparationTime");

                    b.Property<double>("Rating");

                    b.Property<int>("Servings");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("RecipesCore.Models.RecipeIngredient", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("IngredientId");

                    b.Property<string>("Measure");

                    b.Property<string>("Name");

                    b.Property<string>("Quantity");

                    b.Property<long?>("RecipeId");

                    b.HasKey("Id");

                    b.HasIndex("IngredientId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeIngredients");
                });

            modelBuilder.Entity("RecipesCore.Models.User", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Username")
                        .IsRequired();

                    b.Property<bool>("Vegan");

                    b.Property<bool>("Vegetarian");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RecipesCore.Models.UserAllergie", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserAllergies");
                });

            modelBuilder.Entity("RecipesCore.Models.FellowCooks", b =>
                {
                    b.HasOne("RecipesCore.Models.User", "FollowedUser")
                        .WithMany()
                        .HasForeignKey("FollowedUserId");

                    b.HasOne("RecipesCore.Models.User", "User")
                        .WithMany("FellowCooks")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("RecipesCore.Models.Recipe", b =>
                {
                    b.HasOne("RecipesCore.Models.Category", "Category")
                        .WithMany("Recipes")
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("RecipesCore.Models.RecipeIngredient", b =>
                {
                    b.HasOne("RecipesCore.Models.Ingredient", "Ingredient")
                        .WithMany()
                        .HasForeignKey("IngredientId");

                    b.HasOne("RecipesCore.Models.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("RecipesCore.Models.UserAllergie", b =>
                {
                    b.HasOne("RecipesCore.Models.User", "User")
                        .WithMany("Allergies")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
