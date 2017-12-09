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


        public GraphsGenerator(IRecipesService recipesServ)
        {
            _recipesService = recipesServ;
            if (_recipesService == null) Console.Out.WriteLine("RecipesService == NULL");
            
        }

        public void GenerateGraphs()
        {
            //GetRecipeRatingsToCSV();
            string fileName = "Ingrediences.csv";//+filename.Substring(filename.LastIndexOf('\\')

            var recipes = _recipesService.GetNumberOfUsagesIngredience();

            StringBuilder sb = new StringBuilder();

            foreach (var r in recipes)
            {
                Console.Out.WriteLine(r.Item1 + ", " + r.Item2);
                sb.AppendLine(r.Item1 + ", " + r.Item2);
            }
            PrintToFile(fileName, sb);


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
            PrintToFile(fileName, sb);
        }

        public void PrintToFile(string fileName, StringBuilder text)
        {
            FileStream fs = null;
            try
            {

                fs = new FileStream(fileName, FileMode.Truncate);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(text.ToString());
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine("Could not write to file " + fileName);
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }
    }
    
}
