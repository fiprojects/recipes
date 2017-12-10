using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using RecipesCore.Models;
using RecipesCore.Services;

namespace RecipesGraphs
{
    public class GraphsGenerator
    {
        private readonly IRecipesService _recipesService;
        private readonly IIngredientService _ingredientService;


        public GraphsGenerator(IRecipesService recipesServ, IIngredientService ingredientServ)
        {
            _recipesService = recipesServ;
            _ingredientService = ingredientServ;
            if (_recipesService == null) Console.Out.WriteLine("_recipesService == NULL");
            if (_ingredientService == null) Console.Out.WriteLine("_ingredientService == NULL");

        }

        public void GenerateGraphs()
        {
            //GetRecipeRatingsToCSV();
            string fileName = "Ingrediences2.csv";

            var ingredienceIds = _ingredientService.GetAllIngrediencesIds();
            Console.Out.WriteLine(ingredienceIds.Count);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ingredienceId, ingredienceName, numberOfRecipes, importance");
            foreach (long id in ingredienceIds)
            {
                sb = new StringBuilder();

                double numberOfRecipes = _recipesService.GetNumberOfRecipesWithIngredienceByHerId(id);
                string name = _ingredientService.GetIngredienceNameById(id);
                int importance = _ingredientService.GetIngredienceImportanceById(id);

                sb.AppendLine(id + "," + name + "," + numberOfRecipes + "," + importance);
                Console.Out.WriteLine(id + "," + name + "," + numberOfRecipes + "," + importance);
                if (importance == 1)
                {
                    PrintToFile("Ingrediences1.csv", sb, FileMode.Append);
                }
                else
                {
                    PrintToFile("Ingrediences2.csv", sb, FileMode.Append);
                }
                
            }

        }

        private void GetRecipeRatingsToCSV()
        {
            string fileName = "RecipeRatings.csv";//+filename.Substring(filename.LastIndexOf('\\')

            var recipes = _recipesService.GetOrderedAllDownloadedRatings()
                .GroupBy(a => Math.Round(a, 1))
                .Select(group => new {
                    Rating = group.Key,
                    Count = group.Count()
                });

            StringBuilder sb = new StringBuilder();

            foreach (var r in recipes)
            {
                Console.Out.WriteLine(r.Rating + ", " + r.Count);
                sb.AppendLine(r.Rating + ", " + r.Count);
            }
            PrintToFile(fileName, sb, FileMode.Truncate);
        }

        public void PrintToFile(string fileName, StringBuilder text, FileMode mode)
        {
            FileStream fs = null;
            try
            {

                fs = new FileStream(fileName, mode);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(text.ToString());
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine("Could not write to file " + fileName + " "+ text);
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        
    }
    
}
