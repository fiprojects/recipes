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

        void ProcessRecipes(long firstId, long lastId, Action<Recipe> processor);

        List<Recipe> GetRecipes(long firstId, long lastId);
    }
}