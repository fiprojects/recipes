using System;
using System.Collections.Generic;
using RecipesCore;
using RecipesCore.Processors;
using RecipesCore.Services;

namespace TfIdfConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            string time = DateTime.Now.ToString("HH:mm:ss.fff");
            Console.WriteLine("Computing started at " + time);
            ComputeTfIdf();
            time = DateTime.Now.ToString("HH:mm:ss.fff");
            Console.WriteLine("Computing finished at " + time);
        }

        private static void ComputeTfIdf()
        {
            var db = new RecipesContext();
            var recipesService = new RecipesService(db);
            var tfIdfService = new TfIdfService(db);
            IProcessor processor = new TfIdfComputer(recipesService, tfIdfService);
            processor.Run(null);
        }
    }
}
