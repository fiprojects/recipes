using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using NFluent;
using NUnit.Framework;
using RecipesCore.Models;
using RecipesCore.Processors;
using RecipesCore.Services;

namespace RecipesTests
{
    [TestFixture]
    public class TfIdfTest
    {
        private TfIdfComputer _tfIdfComputer;
        
        private Recipe _recipe1;

        private Recipe _recipe2;

        private Recipe _recipe3;
        
        private Recipe _recipe4;
        
        private Mock<IRecipesService> _recipesMock;
        
        private Mock<ITfIdfService> _tfIdfMock;

        [SetUp]
        public void Init()
        {
            InitRecipes();
            
            var recipeList = new List<Recipe> {_recipe1, _recipe2, _recipe3};
            _recipesMock = new Mock<IRecipesService>();
            _recipesMock.Setup(service => service.GetAll()).Returns(recipeList);

            var tfIdfList = GetModelsForThreeRecipes();
            _tfIdfMock = new Mock<ITfIdfService>();
            _tfIdfMock.Setup(service => service.Add(It.IsAny<List<TfIdfModel>>())).Verifiable();
            
            _tfIdfComputer = new TfIdfComputer(_recipesMock.Object, _tfIdfMock.Object);
        }

        private void InitRecipes()
        {
            _recipe1 = new Recipe
            {
                Directions= "\r\nRecipe test A recipe"
            };
            
            _recipe2 = new Recipe();
            string readFromFile = File.ReadAllText("../../../SampleDirections/recipe2.txt");
            _recipe2.Directions = readFromFile;
            
            _recipe3 = new Recipe
            {
                Directions = ""
            };

            _recipe4 = new Recipe
            {
                Directions = "\n recipe a UNIQUE split\r\nsplit"
            };
        }

        [Test]
        public void NoEmptyStringInDictTest()
        {
            var r = new Recipe
            {
                Directions = "something and ."
            };
            var dict = new Dictionary<string, int>();
            dict["something"] = 1;
            dict["and"] = 1;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }

        [Test]
        public void TrimCommaTest()
        {
            var r = new Recipe
            {
                Directions = "something and,    ,and    ,and,    "
            };
            var dict = new Dictionary<string, int>();
            dict["something"] = 1;
            dict["and"] = 3;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void TrimPeriodTest()
        {
            var r = new Recipe
            {
                Directions = "something and.  .and  .and."
            };
            var dict = new Dictionary<string, int>();
            dict["something"] = 1;
            dict["and"] = 3;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void TrimSemicolonTest()
        {
            var r = new Recipe
            {
                Directions = "something and; ;and ;and;"
            };
            var dict = new Dictionary<string, int>();
            dict["something"] = 1;
            dict["and"] = 3;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }

        [Test]
        public void TrimColonTest()
        {
            var r = new Recipe
            {
                Directions = "and :and and: :and:"
            };
            var dict = new Dictionary<string, int>();
            dict["and"] = 4;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void TrimQuestionMarkTest()
        {
            var r = new Recipe
            {
                Directions = "and ?and and? ?and?"
            };
            var dict = new Dictionary<string, int>();
            dict["and"] = 4;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void TrimExclamationMarkTest()
        {
            var r = new Recipe
            {
                Directions = "and !and and! !and!"
            };
            var dict = new Dictionary<string, int>();
            dict["and"] = 4;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void TrimParenthesesTest()
        {
            var r = new Recipe
            {
                Directions = "and (and and( (and( )and and) )and) (and)"
            };
            var dict = new Dictionary<string, int>();
            dict["and"] = 8;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }

        [Test]
        public void TrimBracesTest()
        {
            var r = new Recipe
            {
                Directions = "and {and and{ {and{ }and and} }and} }and}"
            };
            var dict = new Dictionary<string, int>();
            dict["and"] = 8;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void TrimSquareBracketsTest()
        {
            var r = new Recipe
            {
                Directions = "and [and and[ [and[ ]and and] ]and] [and]"
            };
            var dict = new Dictionary<string, int>();
            dict["and"] = 8;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void TrimAngleBracketsTest()
        {
            var r = new Recipe
            {
                Directions = "and <and and< <and< >and and> >and> >and>"
            };
            var dict = new Dictionary<string, int>();
            dict["and"] = 8;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void DoNotTrimInsideWord()
        {
            var r = new Recipe
            {
                Directions = "a(n)d"
            };
            var dict = new Dictionary<string, int>();
            dict["a(n)d"] = 1;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void TermsInLowerCaseTest()
        {
            var r = new Recipe
            {
                Directions = "Something aNd something"
            };
            var dict = new Dictionary<string, int>();
            dict["something"] = 2;
            dict["and"] = 1;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void AllSpacesRemovedTest()
        {
            var r = new Recipe
            {
                Directions = "something  and    something"
            };
            var dict = new Dictionary<string, int>();
            dict["something"] = 2;
            dict["and"] = 1;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void RemovingNewLinesTest()
        {
            var r = new Recipe
            {
                Directions = "something \r\n and something \r\n\r\n and \n something"
            };
            var dict = new Dictionary<string, int>();
            dict["something"] = 3;
            dict["and"] = 2;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }

        [Test]
        public void RemovingMoreCharacters()
        {
            var r = new Recipe
            {
                Directions = "?!and And;,. :!(aNd)?"
            };
            var dict = new Dictionary<string, int>();
            dict["and"] = 3;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void NewLineSplitWords()
        {
            var dict = new Dictionary<string, int>();
            dict["recipe"] = 1;
            dict["a"] = 1;
            dict["unique"] = 1;
            dict["split"] = 2;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(_recipe4);
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void TfIdfOnTwoRecipesTest()
        {

            var list = new List<Recipe>
            {
                _recipe1,
                _recipe3
            };
            var expectedModel = new TfIdfModel
            {
                Recipe = _recipe1
            };
            expectedModel.Elements.Add(new TfIdfElement{Id = 0, Term = "recipe", TfIdf = Math.Log(2, 10)});
            expectedModel.Elements.Add(new TfIdfElement{Id = 0, Term = "test", TfIdf = 0.5 * Math.Log(2, 10)});
            expectedModel.Elements.Add(new TfIdfElement{Id = 0, Term = "a", TfIdf = 0.5 * Math.Log(2, 10)});
            var expectedList = new List<TfIdfModel>
            {
                expectedModel
            };
            var ret = _tfIdfComputer.ComputeTfIdfForRecipes(list);
            ModelsAreTheSame(expectedList, ret);
        }

        [Test]
        public void TfIdfOnThreeRecipesTest()
        {
            var list = new List<Recipe>
            {
                _recipe1,
                _recipe2,
                _recipe3
            };
            var expectedList = GetModelsForThreeRecipes();
            var ret = _tfIdfComputer.ComputeTfIdfForRecipes(list);
            ModelsAreTheSame(expectedList, ret);

        }

        [Test]
        public void TfIdfCommonWordsInAllRecipesTest()
        {
            var recipeList = new List<Recipe>
            {
                _recipe1,
                _recipe4
            };
            
            var model1 = new TfIdfModel
            {
                Recipe = _recipe1
            };
            model1.Elements.Add(new TfIdfElement{Term = "recipe", TfIdf = 0.0});
            model1.Elements.Add(new TfIdfElement{Term = "test", TfIdf = 0.5 * Math.Log(2 ,10)});
            model1.Elements.Add(new TfIdfElement{Term = "a", TfIdf = 0.0});
            
            var model2 = new TfIdfModel
            {
                Recipe = _recipe4
            };
            model2.Elements.Add(new TfIdfElement{Term = "recipe", TfIdf = 0.0});
            model2.Elements.Add(new TfIdfElement{Term = "a", TfIdf = 0.0});
            model2.Elements.Add(new TfIdfElement{Term = "unique", TfIdf = 0.5 * Math.Log(2, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "split", TfIdf = 1 * Math.Log(2, 10)});
            
            var expectedList = new List<TfIdfModel>
            {
                model1,
                model2
            };
            var ret = _tfIdfComputer.ComputeTfIdfForRecipes(recipeList);
            ModelsAreTheSame(expectedList, ret);
        }

        [Test]
        [Ignore("Not all features are implemented yet")]
        public void TfIdfOnTwoRealDirectionsTest()
        {
            var recipeList = GetTwoRealRecipes();
            var expectedModels = GetExpectedModelsForTfIdfOnTwoRealDirectionsTest(recipeList);
            var returnedModels = _tfIdfComputer.ComputeTfIdfForRecipes(recipeList);
            ModelsAreTheSame(expectedModels, returnedModels);
        }

        private List<TfIdfModel> GetExpectedModelsForTfIdfOnTwoRealDirectionsTest(List<Recipe> recipesList,
            double n = 2.0)
        {
            double threshold = 0.000001;
            if ((n - 2.0 < threshold) || (n - 3.0 < threshold))
                throw new ArgumentException("n");
            var model1 = new TfIdfModel
            {
                Recipe = recipesList[0]
            };
            model1.Elements.Add(new TfIdfElement{Term = "preheat", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "the", TfIdf = (9.0/9.0) * Math.Log(n / 2.0 ,10)});
            model1.Elements.Add(new TfIdfElement{Term = "oven", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "on", TfIdf = (2.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "broiler", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "setting", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "in", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "a", TfIdf = (3.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "large", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "bowl", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "combine", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "roma", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "tomatoes", TfIdf = (2.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "sun-dried", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "garlic", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "olive", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "oil", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "vinegar", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "basil", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "salt", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "and", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "pepper", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "allow", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "mixture", TfIdf = (2.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "to", TfIdf = (2.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "sit", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "for", TfIdf = (3.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "10", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "minutes", TfIdf = (3.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "cut", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "baguette", TfIdf = (3.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "into", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "3/4-inch", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "slices", TfIdf = (4.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "baking", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "sheet", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "arrange", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "single", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "layer", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "broil", TfIdf = (2.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "1", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "2", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "5", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "until", TfIdf = (2.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "slightly", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "brown", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "divide", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "tomato", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "evenly", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "over", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "top", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "with", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "mozzarella", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "cheese", TfIdf = (2.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "or", TfIdf = (1.0/9.0) * Math.Log(n / 2.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "is", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});
            model1.Elements.Add(new TfIdfElement{Term = "melted", TfIdf = (1.0/9.0) * Math.Log(n / 1.0,10)});

            var model2 = new TfIdfModel
            {
                Recipe = recipesList[1]
            };

            model2.Elements.Add(new TfIdfElement{Term = "preheat", TfIdf = (1.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "oven", TfIdf = (3.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "to", TfIdf = (2.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "350", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "degrees", TfIdf = (2.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "f", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "175", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "c", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "dip", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "eggplant", TfIdf = (2.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "slices", TfIdf = (2.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "in", TfIdf = (7.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "egg", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "then", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "bread", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "crumbs", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "place", TfIdf = (2.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "a", TfIdf = (4.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "single", TfIdf = (1.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "layer", TfIdf = (2.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "on", TfIdf = (3.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "baking", TfIdf = (2.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "sheet", TfIdf = (1.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "bake", TfIdf = (2.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "preheated", TfIdf = (2.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "for", TfIdf = (2.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "5", TfIdf = (1.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "minutes", TfIdf = (2.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "each", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "side", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "9x13", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "inch", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "dish", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "spread", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "spaghetti", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "sauce", TfIdf = (2.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "cover", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "the", TfIdf = (3.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "bottom", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "of", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "sprinkle", TfIdf = (2.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "with", TfIdf = (3.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "mozzarella", TfIdf = (1.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "and", TfIdf = (1.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "parmesan", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "cheeses", TfIdf = (2.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "repeat", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "remaining", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "ingredients", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "ending", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "basiltop", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "35", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "or", TfIdf = (1.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "until", TfIdf = (1.0/7.0) * Math.Log(n / 2.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "golden", TfIdf = (1.0/7.0) * Math.Log(n / 1.0,10)});
            model2.Elements.Add(new TfIdfElement{Term = "brown", TfIdf = (1.0/7.0) * Math.Log(n / 2.0,10)});
            
            var retList = new List<TfIdfModel> {model1, model2};
            return retList;
        }

        private List<Recipe> GetTwoRealRecipes()
        {
            string readFromFile = File.ReadAllText("../../../SampleDirections/double_tomato_bruschetta_directions.txt");
            var r1 = new Recipe
            {
                Directions = readFromFile
            };
            readFromFile = File.ReadAllText("../../../SampleDirections/egg_plant_parmesan_II.txt");
            var r2 = new Recipe
            {
                Directions = readFromFile
            };
            return new List<Recipe>{r1, r2};
        }

        [Test]
        public void RunTest()
        {
            _tfIdfComputer.Run(null);
            _recipesMock.Verify(service => service.GetAll());
            var modelsList = GetModelsForThreeRecipes();
            _tfIdfMock.VerifyAll();
        }

        private List<TfIdfModel> GetModelsForThreeRecipes()
        {
            var model1 = new TfIdfModel
            {
                Recipe = _recipe1
            };
            model1.Elements.Add(new TfIdfElement{Term = "recipe", TfIdf = Math.Log((double)3/1, 10)});
            model1.Elements.Add(new TfIdfElement{Term = "test", TfIdf = 0.5 * Math.Log((double)3/1, 10)});
            model1.Elements.Add(new TfIdfElement{Term = "a", TfIdf = 0.5 * Math.Log((double)3/2, 10)});

            var model2 = new TfIdfModel
            {
                Recipe = _recipe2
            };
            model2.Elements.Add(new TfIdfElement{Term = "rinse", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "buckwheat", TfIdf = ((double)3/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "groats", TfIdf = ((double)2/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "bring", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "a", TfIdf = ((double)2/3) * Math.Log((double)3/2, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "saucepan", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "of", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "water", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "to", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "boil", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "sprinkle", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "in", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "the", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "and", TfIdf = ((double)2/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "simmer", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "until", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "is", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "tender", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "about", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "10", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "minutes", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "drain", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});
            model2.Elements.Add(new TfIdfElement{Term = "cool", TfIdf = ((double)1/3) * Math.Log((double)3/1, 10)});

            var ret = new List<TfIdfModel>
            {
                model1,
                model2
            };
            return ret;
        }

        private void ModelsAreTheSame(List<TfIdfModel> expectedList, List<TfIdfModel> returnedList)
        {
            Assert.NotNull(expectedList);
            Assert.NotNull(returnedList);
            double threshold = 1e-06;
            Assert.AreEqual(expectedList.Count, returnedList.Count);
            foreach (var model in expectedList)
            {
                var retModel = returnedList.Find(m => m.Recipe.Equals(model.Recipe));
                Assert.NotNull(retModel);
                Assert.AreEqual(model.Elements.Count, retModel.Elements.Count);
                foreach (var element in model.Elements)
                {
                    var retElement = retModel.Elements.Find(e => e.Term.Equals(element.Term));
                    Assert.NotNull(retElement);
                    double diff = Math.Abs(element.TfIdf - retElement.TfIdf);
                    Assert.LessOrEqual(diff, threshold);
                }
            }
        }
    }
}