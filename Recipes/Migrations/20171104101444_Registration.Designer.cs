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
    [Migration("20171104101444_Registration")]
    partial class Registration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("RecipesCore.Models.Recipe", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<int>("Calories");

                    b.Property<string>("Category");

                    b.Property<TimeSpan>("CookTime");

                    b.Property<string>("Description");

                    b.Property<string>("Directions");

                    b.Property<byte[]>("Image");

                    b.Property<string>("Name");

                    b.Property<TimeSpan>("PreparationTime");

                    b.Property<double>("Rating");

                    b.Property<int>("Servings");

                    b.HasKey("Id");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("RecipesCore.Models.RecipeIngredient", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<long?>("RecipeId");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeIngredient");
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

            modelBuilder.Entity("RecipesCore.Models.RecipeIngredient", b =>
                {
                    b.HasOne("RecipesCore.Models.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId");
                });
#pragma warning restore 612, 618
        }
    }
}
