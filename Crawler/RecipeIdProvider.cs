using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crawler
{
    public class RecipeIdProvider
    {
        public static Dictionary<long, string> CategoryMap { get; } = new Dictionary<long, string>();

        public static IEnumerable<long> CategorizedCsv(string file)
        {
            CategoryMap.Clear();

            var recipes = new List<long>();
            var lines = File.ReadAllLines(file);
            foreach (var line in lines)
            {
                var data = line.Split(';');
                var id = long.Parse(data[0]);

                recipes.Add(id);
                CategoryMap.Add(id, data[1]);
            }

            return recipes;
        }

        public static IEnumerable<long> Range(int first, int last)
        {
            return Enumerable.Range(first, last)
                .Select(x => (long) x);
        }

        public static IEnumerable<long> Random(int first, int last, int num)
        {
            num = Math.Min(last - first, num);
            var ids = new HashSet<long>();
            var random = new Random();

            while (ids.Count < num)
            {
                ids.Add(random.Next(first, last));
            }

            return ids;
        }
    }
}