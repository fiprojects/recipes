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
        private readonly ITfIdfService _tfIdfService;

        public GraphsGenerator(IRecipesService recipesServ, IIngredientService ingredientServ, ITfIdfService tfidfServ)
        {
            _recipesService = recipesServ;
            _ingredientService = ingredientServ;
            _tfIdfService = tfidfServ;
            if (_recipesService == null) Console.Out.WriteLine("_recipesService == NULL");
            if (_ingredientService == null) Console.Out.WriteLine("_ingredientService == NULL");
            if (_tfIdfService == null) Console.Out.WriteLine("_tfIdfService == NULL");
        }

        public void GenerateGraphs()
        {
            //GetRecipeRatingsToCSV();
            //GetFrewOfIngrediencesToCSV();
            //GetCookTimesToCSV();
            //GetPreparationTimesToCSV();
            //GetPreparationAndCookTimesToCSV();
            //GetTfidfDataToCSV();
            GetCountOfRecipesForCategoriesToCSV();
            
        }

        private void GetCountOfRecipesForCategoriesToCSV()
        {
            List<Tuple<string, int>> categoriesAndRecipes = _recipesService.GetCountOfRecipesInCategory();
            string fileName = "CategoriesAndRecipes.csv";
            StringBuilder sb = new StringBuilder("Category, numberOfRecipes" + Environment.NewLine);
            PrintToFile(fileName, sb, FileMode.Append);
            

            foreach (var categoryAndCount in categoriesAndRecipes)
            {
                sb = new StringBuilder();


                sb.AppendLine(categoryAndCount.Item1 + "," + categoryAndCount.Item2);
                Console.Out.WriteLine(categoryAndCount.Item1 + "," + categoryAndCount.Item2);
                PrintToFile(fileName, sb, FileMode.Append);

            }

        }

        private void GetTfidfDataToCSV()
        {
            string fileName = "TFIDFcsvForGraph.csv";

            List<Tuple<string, int>> termnAndCountOfRecipes = _tfIdfService.GetNumberOfRecipesWhereUsedForTerms();
            StringBuilder sb = new StringBuilder();
            foreach (var termAndCount in termnAndCountOfRecipes)
            {
                sb = new StringBuilder();


                sb.AppendLine(termAndCount.Item1 + "," + termAndCount.Item2);
                Console.Out.WriteLine(termAndCount.Item1 + "," + termAndCount.Item2);
                PrintToFile(fileName, sb, FileMode.Append);

            }
            sb.AppendLine("Number recipes in which used, number of terms");
            PrintToFile(fileName, sb, FileMode.Append);

            List<Tuple<int, int>> numOfRecipesAndCountOfTerms =
                _tfIdfService.GetSumOfTermsUsedInTheSameNumberOfRecipes();
            
            foreach (var numOfRecipesAndCountT in numOfRecipesAndCountOfTerms)
            {
                sb = new StringBuilder();


                sb.AppendLine(numOfRecipesAndCountT.Item1 + "," + numOfRecipesAndCountT.Item2);
                Console.Out.WriteLine(numOfRecipesAndCountT.Item1 + "," + numOfRecipesAndCountT.Item2);
                PrintToFile(fileName, sb, FileMode.Append);

            }
            List<Tuple<String, int>> groupedTupleList =  new List<Tuple<String, int>>();
            Tuple<String, int> tuple = null;
            string name = "";
            for (int i = 1; i <= 450; i++)
            {
                
                if (i % 3 == 1)
                {
                    tuple = null;
                    name = i + "..." + (i + 2);
                }
                Tuple<int, int> foundTuple = numOfRecipesAndCountOfTerms.Find(a => a.Item1 == i);
                if (foundTuple != null)
                {
                    int value = 0;
                    
                    if (tuple != null )
                    {
                        value = tuple.Item2;
                    }

                    tuple = new Tuple<string, int>(name, foundTuple.Item2 + value);
                    
                }
                if (i % 3 == 0 && tuple != null)
                {
                    groupedTupleList.Add(tuple);
                    Console.Out.WriteLine(tuple.Item1 + ", " + tuple.Item2);
                    sb = new StringBuilder(tuple.Item1 + ", " + tuple.Item2 +  Environment.NewLine);
                    PrintToFile(fileName, sb, FileMode.Append);
                }

            }
        }

        private void GetCookTimesToCSV()
        {
            string fileName = "CookTime.csv";

            List<Tuple<int, int>> cookTimes = _recipesService.GetCookingTimesAndRecipesCount();
            StringBuilder sb = new StringBuilder();
            foreach (var cookT in cookTimes)
            {
                sb = new StringBuilder();


                sb.AppendLine(cookT.Item1 + "," + cookT.Item2);
                Console.Out.WriteLine(cookT.Item1 + "," + cookT.Item2);
                PrintToFile(fileName, sb, FileMode.Append);

            }
        }

        private void GetPreparationTimesToCSV()
        {
            string fileName = "PreparationTime.csv";

            List<Tuple<int, int>> cookTimes = _recipesService.GetPreparationTimesAndRecipesCount();
            StringBuilder sb = new StringBuilder();
            foreach (var cookT in cookTimes)
            {
                sb = new StringBuilder();


                sb.AppendLine(cookT.Item1 + "," + cookT.Item2);
                Console.Out.WriteLine(cookT.Item1 + "," + cookT.Item2);
                PrintToFile(fileName, sb, FileMode.Append);

            }
        }

        private void GetPreparationAndCookTimesToCSV()
        {
            string fileName = "PreparationAndCookTime.csv";

            List<Tuple<int, int>> cookTimes = _recipesService.GetCookAndPrepTimesAndRecipesCount();
            StringBuilder sb = new StringBuilder();
            foreach (var cookT in cookTimes)
            {
                sb = new StringBuilder();


                sb.AppendLine(cookT.Item1 + "," + cookT.Item2);
                Console.Out.WriteLine(cookT.Item1 + "," + cookT.Item2);
                PrintToFile(fileName, sb, FileMode.Append);

            }
        }

        private void GetFrewOfIngrediencesToCSV()
        {
            var ingredienceIds = _ingredientService.GetAllIngrediencesIds();
            Console.Out.WriteLine(ingredienceIds.Count);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ingredienceId, ingredienceName, numberOfRecipes, importance");
            foreach (long id in ingredienceIds)
            {
                sb = new StringBuilder();

                double numberOfRecipes = _recipesService.GetNumberOfRecipesWithIngredienceById(id);
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
