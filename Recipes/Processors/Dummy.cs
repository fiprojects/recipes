using System;

namespace RecipesCore.Processors
{
    public class Dummy : IProcessor
    {
        public void Run(string[] args)
        {
            Console.WriteLine($"Hello {args[0]}");
        }
    }
}