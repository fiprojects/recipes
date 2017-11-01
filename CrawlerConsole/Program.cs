using System.IO;
using System.Reflection;
using System.Xml;
using Crawler;
using log4net;
using RecipesCore;
using RecipesCore.Services;

namespace CrawlerConsole
{
    public class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            SetupLogging();
            CrawlToDatabase();
        }

        private static void SetupLogging()
        {
            var config = new XmlDocument();
            config.Load(File.OpenRead("log4net.config"));

            var repository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repository, config["log4net"]);
        }

        private static void CrawlToDatabase()
        {
            var db = new RecipesContext();
            var recipesService = new RecipesService(db);

            ICrawler crawler = new AllRecipesCrawler { Logger = Log };
            // crawler.ProcessRecipes(RecipeIdProvider.Range(6800, 6810), recipesService.Add);
            crawler.ProcessRecipes(RecipeIdProvider.Random(6800, 300000, 20), recipesService.Add);
        }
    }
}
