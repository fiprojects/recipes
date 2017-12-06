using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using RecipesCore;
using RecipesCore.Services;

namespace RecipesConsole
{
    public class RecipesConsole
    {
        private readonly IServiceProvider _serviceProvider;

        public RecipesConsole()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddRecipesCore();
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Run(string[] args)
        {
            switch (GetArgument(args, 0))
            {
                case "build":
                    Build(args);
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

        private static string GetArgument(IReadOnlyList<string> args, int index)
        {
            if (index >= args.Count)
            {
                return null;
            }

            return args[index].ToLower();
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