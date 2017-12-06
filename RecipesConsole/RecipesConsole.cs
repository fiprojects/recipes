using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Crawler;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using RecipesCore;
using RecipesCore.Services;

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

                default:
                    Console.WriteLine("Invalid action.");
                    break;
            }
        }

        public void Build(string[] args)
        {
            GetArgument(args, 1, out var first);
            GetArgument(args, 2, out var second);
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

        public void Crawl()
        {
            var recipesService = _serviceProvider.GetService<IRecipesService>();

            ICrawler crawler = new AllRecipesCrawler { Logger = Log };
            crawler.ProcessRecipes(RecipeIdProvider.CategorizedCsv("recipes.csv"), recipesService.Add);
        }

        private static void SetupLogging()
        {
            var config = new XmlDocument();
            config.Load(File.OpenRead("log4net.config"));

            var repository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repository, config["log4net"]);
        }

        private static string GetArgument(IReadOnlyList<string> args, int index)
        {
            return index < args.Count ? args[index].ToLower() : null;
        }

        private static void GetArgument(IReadOnlyList<string> args, int index, out string value)
        {
            value = GetArgument(args, index);
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