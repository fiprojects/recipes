using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RecipesCore.Info
{
    public static class RecommendingAlgorithms
    {
        private static readonly Dictionary<string, RecommendingAlgorithm> Algorithms = new Dictionary<string, RecommendingAlgorithm>();

        static RecommendingAlgorithms()
        {
            Add("Random", "Random");
            Add("Ingredients", "Ingredients");
            Add("TfIdf", "TF-IDF");
        }

        public static ReadOnlyCollection<RecommendingAlgorithm> GetAll()
        {
            return Algorithms.Values.ToList().AsReadOnly();
        }

        public static RecommendingAlgorithm Get(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return null;
            }

            Algorithms.TryGetValue(identifier, out var algorithm);
            return algorithm;
        }

        private static void Add(string identifier, string name)
        {
            Algorithms.Add(identifier, new RecommendingAlgorithm(identifier, name));
        }
    }
}