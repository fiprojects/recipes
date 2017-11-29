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
                Directions = "something and,"
            };
            var dict = new Dictionary<string, int>();
            dict["something"] = 1;
            dict["and"] = 1;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            Assert.False(dict.ContainsKey("and,"));
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void TrimPeriodTest()
        {
            var r = new Recipe
            {
                Directions = "something and. "
            };
            var dict = new Dictionary<string, int>();
            dict["something"] = 1;
            dict["and"] = 1;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            Assert.False(dict.ContainsKey("and."));
            CollectionAssert.AreEquivalent(dict, ret);
        }
        
        [Test]
        public void TrimSemicolonTest()
        {
            var r = new Recipe
            {
                Directions = "something and; "
            };
            var dict = new Dictionary<string, int>();
            dict["something"] = 1;
            dict["and"] = 1;
            var ret = _tfIdfComputer.GetTermsWithCountForRecipe(r);
            Assert.False(dict.ContainsKey("and;"));
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
        public void TermsInLowerCaseTestTest()
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
        public void TfIdfCommonWordsInAllRecipes()
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