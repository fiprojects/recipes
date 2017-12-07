using System;
using System.Linq;
using RecipesCore.Models;

namespace RecipesCore.Processors
{
    public class IngredientsParser : IProcessor
    {
        private readonly RecipesContext _db = new RecipesContext();
        private readonly string[] _recognizedIngredients = {
            "allspice",
            "almond",
            "anchovy",
            "apple",
            "apricot",
            "artichoke",
            "asparagus",
            "avocado",
            "bacon",
            "baking powder",
            "baking soda",
            "bamboo",
            "banana",
            "barley",
            "basil",
            "bay leaf",
            "beans",
            "beer",
            "blueberries",
            "broccoli",
            "bulgur",
            "butter",
            "cabbage",
            "capers",
            "carrot",
            "cashew",
            "cauliflower",
            "cayenne",
            "celery",
            "cheese",
            "chickpeas",
            "chili",
            "chive",
            "cilantro",
            "cinnamon",
            "clam",
            "clove",
            "cocoa",
            "coconut",
            "cod",
            "coffee",
            "condensed milk",
            "coriander",
            "cranberries",
            "crab",
            "crust",
            "cucumber",
            "cumin",
            "curry",
            "dates",
            "dill",
            "dough",
            "dressing",
            "duck",
            "egg",
            "fennel",
            "flour",
            "garam masala",
            "garlic",
            "ginger",
            "grapes",
            "ham",
            "heavy cream",
            "heavy whipping cream",
            "honey",
            "hummus",
            "jam",
            "kale",
            "ketchup",
            "kiwi",
            "lamb",
            "lasagna",
            "leek",
            "lemon",
            "lentils",
            "lettuce",
            "lime",
            "lobster",
            "macaroni",
            "marjoram",
            "mango",
            "maple syrup",
            "margarine",
            "mayonnaise",
            "milk",
            "mint",
            "molasses",
            "mozzarella",
            "mushroom",
            "mussel",
            "mustard",
            "noodles",
            "nutmeg",
            "oats",
            "olive",
            "onion",
            "orange",
            "oregano",
            "paprika",
            "parsley",
            "pasta",
            "peach",
            "pear",
            "peas",
            "peanuts",
            "pecans",
            "pepper",
            "pesto",
            "pita",
            "poppy",
            "pork",
            "potatoes",
            "pumpkin",
            "quinoa",
            "radish",
            "raisins",
            "raspberries",
            "relish",
            "rice",
            "rosemary",
            "rum",
            "saffron",
            "sage",
            "sake",
            "salad",
            "salmon",
            "salsa",
            "salt",
            "sausage",
            "scallop",
            "seashell",
            "seasoning",
            "sesame",
            "sherry",
            "sour cream",
            "spaghetti",
            "spinach",
            "squash",
            "sriracha",
            "shallot",
            "shortening",
            "shrimp",
            "steak",
            "strawberries",
            "tahini",
            "tamarind",
            "tarragon",
            "tequila",
            "thyme",
            "tofu",
            "tortilla",
            "turkey",
            "turmeric",
            "turnip",
            "tuna",
            "vanilla extract",
            "vegetable broth",
            "vodka",
            "water",
            "whiskey",
            "wine",
            "wonton",
            "yeast",
            "yogurt",
            "zucchini",

            "beef broth",
            "beef",

            "bread crumbs",
            "bread",

            "chicken broth",
            "chicken",

            "cornstarch",
            "corn",

            "cream",

            "olive oil",
            "oil",

            "fish sauce",
            "soy sauce",
            "hoisin sauce",
            "sauce",

            "brown sugar",
            "sugar",

            "tomato sauce",
            "tomato paste",
            "tomato",

            "balsamic vinegar",
            "rice vinegar",
            "white vinegar",
            "vinegar",

            "walnuts",
            "nuts"
        };

        public void Run(string[] args)
        {
            foreach (var recognizedIngredient in _recognizedIngredients)
            {
                var recipeIngredients = _db.RecipeIngredients
                    .Where(r => r.Ingredient == null && r.Name.Contains(recognizedIngredient));
                foreach (var recipeIngredient in recipeIngredients)
                {
                    Console.WriteLine(recipeIngredient.Name);

                    var ingredient = CreateOrGetIngredient(recognizedIngredient);
                    recipeIngredient.Ingredient = ingredient;
                }
            }

            _db.SaveChanges();
        }

        private Ingredient CreateOrGetIngredient(string name)
        {
            var ingredient = _db.Ingredients.SingleOrDefault(i => i.Name == name);
            if (ingredient == null)
            {
                ingredient = new Ingredient { Name = name };
                _db.Add(ingredient);
                _db.SaveChanges();
            }

            return ingredient;
        }
    }
}