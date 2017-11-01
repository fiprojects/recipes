using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Crawler
{
    public class RecipeSource
    {
        private static readonly Regex DateTimeRegex = new Regex(@"^PT((?<hour>\d+)H)?((?<minute>\d+)M)?$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly HtmlDocument _document = new HtmlDocument();
        private readonly HtmlNode _root;

        public bool IsValid => _root.SelectSingleNode("//section[@class='error-page']") == null;

        public string Name => GetText("//h1");

        public string Description => GetText("//div[@itemprop='description']").Trim('\r', '\n', ' ', '"');

        public List<string> Categories => GetCategories();

        public string Author => GetText("//span[@itemprop='author']");

        public double Rating => GetRating();

        public TimeSpan PreparationTime => GetTime("prepTime");

        public TimeSpan CookTime => GetTime("cookTime");

        public int Servings => GetServings();

        public int Calories => GetCalories();

        public List<string> Ingredients => GetIngredients();

        public string Directions => GetDirections();

        public string ImageUrl => GetImageUrl();

        public RecipeSource(string html)
        {
            _document.LoadHtml(html);
            _root = _document.DocumentNode;
        }

        private string GetText(string xpath) => HtmlEntity.DeEntitize(_root.SelectSingleNode(xpath)?.InnerText);

        private List<string> GetCategories()
        {
            var categories = _root.SelectNodes("//span[@itemprop='name']");
            if (categories == null || categories.Count == 0)
            {
                return new List<string>();
            }

            return categories
                .Select(ingredient => ingredient.InnerText.Trim())
                .Skip(2)
                .ToList();
        }

        private double GetRating()
        {
            var ratingString = _root.SelectSingleNode("//div[@class='rating-stars']")
                ?.Attributes["data-ratingstars"]
                ?.Value;
            return !double.TryParse(ratingString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var rating)
                ? 0d
                : rating;
        }

        private TimeSpan GetTime(string type)
        {
            var timeString = _root.SelectSingleNode($"//time[@itemprop='{type}']")
                ?.Attributes["datetime"]
                ?.Value;
            if (string.IsNullOrWhiteSpace(timeString))
            {
                return new TimeSpan();
            }

            var matches = DateTimeRegex.Match(timeString);
            var hourString = matches.Groups["hour"];
            var minuteString = matches.Groups["minute"];

            var hours = 0;
            var minutes = 0;

            if (!string.IsNullOrWhiteSpace(hourString.Value))
            {
                int.TryParse(hourString.Value, out hours);
            }

            if (!string.IsNullOrWhiteSpace(minuteString.Value))
            {
                int.TryParse(minuteString.Value, out minutes);
            }

            return new TimeSpan(hours, minutes, 0);
        }

        private List<string> GetIngredients()
        {
            var ingredients = _root.SelectNodes("//span[@itemprop='ingredients']");
            if (ingredients == null || ingredients.Count == 0)
            {
                return new List<string>();
            }

            return ingredients.Select(ingredient => ingredient.InnerText).ToList();
        }

        private int GetServings()
        {
            var servingsString = _root.SelectSingleNode("//meta[@id='metaRecipeServings']");
            return int.TryParse(servingsString?.Attributes["content"]?.Value, out var servings) ? servings : 0;
        }

        private int GetCalories()
        {
            var caloriesString = _root.SelectSingleNode("//span[@class='calorie-count']/span");
            return int.TryParse(caloriesString?.InnerText, out var calories) ? calories : 0;
        }

        private string GetDirections()
        {
            var stringBuilder = new StringBuilder();

            var steps = _root.SelectNodes("//li[@class='step']");
            if (steps == null || steps.Count == 0)
            {
                return string.Empty;
            }

            foreach (var step in steps)
            {
                stringBuilder.AppendLine(HtmlEntity.DeEntitize(step.InnerText));
            }

            return stringBuilder.ToString();
        }

        private string GetImageUrl()
        {
            var img = _root.SelectSingleNode("//img[@class='rec-photo']");
            return img?.Attributes["src"].Value;
        }
    }
}