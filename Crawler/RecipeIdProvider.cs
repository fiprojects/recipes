using System;
using System.Collections.Generic;
using System.Linq;

namespace Crawler
{
    public class RecipeIdProvider
    {
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