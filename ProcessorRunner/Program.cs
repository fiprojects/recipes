using System;
using System.Linq;
using RecipesCore.Processors;

namespace ProcessorRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Processor must be specified.");
                return;
            }

            var passArgs = args.Skip(1).ToArray();
            try
            {
                var type = Type.GetType($"RecipesCore.Processors.{args[0]}, RecipesCore");
                var processor = (IProcessor) Activator.CreateInstance(type);
                processor.Run(passArgs);
            }
            catch (Exception)
            {
                Console.WriteLine("Processor cannot be run.");
            }
        }
    }
}
