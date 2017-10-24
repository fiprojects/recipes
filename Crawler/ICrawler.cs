using System.Collections.Generic;
using log4net;
using RecipesCore;

namespace Crawler
{
    public interface ICrawler
    {
        ILog Logger { get; set; }

        Recipe GetRecipe(long id);

        List<Recipe> GetRecipes(long firstId, long lastId);
    }
}