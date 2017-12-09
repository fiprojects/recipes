using System;
using System.Collections.Generic;

namespace RecipesCore.Info
{
    public class RecommendingAlgorithm
    {
        public string Identifier { get; }

        public string Name { get; }

        public RecommendingAlgorithm(string identifier, string name)
        {
            Identifier = identifier;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is RecommendingAlgorithm algorithm
                && string.Equals(Identifier, algorithm.Identifier, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            var hashCode = 1452607508 + EqualityComparer<string>.Default.GetHashCode(Identifier);
            return hashCode;
        }
    }
}