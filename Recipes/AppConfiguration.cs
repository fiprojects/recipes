using Microsoft.Extensions.Configuration;

namespace RecipesCore
{
    public class AppConfiguration
    {
        private readonly IConfigurationRoot _configurationRoot;

        public string this[string key] => _configurationRoot[key];

        public AppConfiguration(string file)
        {
            _configurationRoot = new ConfigurationBuilder()
                .AddJsonFile(file)
                .Build();
        }
    }
}