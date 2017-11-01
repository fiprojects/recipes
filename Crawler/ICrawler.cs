using System;
using System.Collections.Generic;
using log4net;
using RecipesCore.Models;

namespace Crawler
{
    public interface ICrawler
    {
        ILog Logger { get; set; }

        Recipe GetRecipe(long id);

        void ProcessRecipes(IEnumerable<long> ids, Action<Recipe> processor);

        List<Recipe> GetRecipes(IEnumerable<long> ids);
    }
}