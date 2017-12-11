using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Crawler;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using RecipesCore;
using RecipesCore.Processors;
using RecipesCore.Services;
using RecipesGraphs;


namespace RecipesConsole
{
    public class RecipesConsole
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
        private readonly IServiceProvider _serviceProvider;

        public RecipesConsole()
        {
            SetupLogging();

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddRecipesCore();
            serviceCollection.AddDbContext<RecipesContext>();
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Run(string[] args)
        {
            switch (GetArgument(args, 0))
            {
                case "build":
                    Build(args);
                    break;

                case "crawl":
                    Crawl();
                    break;

                case "processor":
                    Processor(args);
                    break;

                case "tfidf":
                    TfIdf();
                    break;

                case "graphs":
                    GenerateGraphs();
                    break;

                default:
                    Console.WriteLine("Invalid action.");
                    break;
            }
        }

        private void GenerateGraphs()
        {
            var recipesService = _serviceProvider.GetService<IRecipesService>();
            var ingredientService = _serviceProvider.GetService<IIngredientService>();
            var TfIdfService = _serviceProvider.GetService<ITfIdfService>();
            GraphsGenerator graphGen = new GraphsGenerator(recipesService, ingredientService, TfIdfService);
            graphGen.GenerateGraphs();
        }

        private void Build(string[] args)
        {
            var first = GetArgument(args, 1);
            var second = GetArgument(args, 2);
            switch (first)
            {
                case "image-cache":
                    RequireArgument(second);
                    _serviceProvider.GetService<IImageService>().BuildCache(second);
                    break;

                case "thumbnails":
                    RequireArgument(second);
                    _serviceProvider.GetService<IImageService>().BuildThumbnails(second);
                    break;

                default:
                    Console.WriteLine("Invalid action.");
                    break;
            }
        }

        private void Crawl()
        {
            var recipesService = _serviceProvider.GetService<IRecipesService>();

            ICrawler crawler = new AllRecipesCrawler { Logger = Log };
            crawler.ProcessRecipes(RecipeIdProvider.CategorizedCsv("recipes.csv"), recipesService.Add);
        }

        private void Processor(string[] args)
        {
            var processorName = GetArgument(args, 1, false);
            var passArgs = args.Skip(2).ToArray();
            try
            {
                var type = Type.GetType($"RecipesCore.Processors.{processorName}, RecipesCore");
                var processor = (IProcessor) Activator.CreateInstance(type);
                processor.Run(passArgs);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Processor cannot be run: {e.Message}");
            }
        }

        private void TfIdf()
        {
            Log.Debug($"TF-IDF: Computing started at {DateTime.Now:HH:mm:ss.fff}");

            var recipesService = _serviceProvider.GetService<IRecipesService>();
            var tfIdfService = _serviceProvider.GetService<ITfIdfService>();
            new TfIdfComputer(recipesService, tfIdfService, "../Recipes/stop_words.json").Run(null);
    
            Log.Debug($"TF-IDF: Computing finished at {DateTime.Now:HH:mm:ss.fff}");
        }

        private static void SetupLogging()
        {
            var config = new XmlDocument();
            config.Load(File.OpenRead("log4net.config"));

            var repository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repository, config["log4net"]);
        }

        private static string GetArgument(IReadOnlyList<string> args, int index, bool normalize = true)
        {
            if (index >= args.Count)
            {
                return null;
            }

            return normalize ? args[index].ToLower() : args[index];
        }

        private static void RequireArgument(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            Console.WriteLine("Parameter is required.");
            Environment.Exit(-1);
        }
    }
}