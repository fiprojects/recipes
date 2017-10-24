using System.IO;
using System.Reflection;
using System.Xml;
using Crawler;
using log4net;

namespace CrawlerConsole
{
    public class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            SetupLogging();

            ICrawler crawler = new AllRecipesCrawler { Logger = Log };
            crawler.GetRecipes(6800, 6900);
        }

        private static void SetupLogging()
        {
            var config = new XmlDocument();
            config.Load(File.OpenRead("log4net.config"));

            var repository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repository, config["log4net"]);
        }
    }
}
