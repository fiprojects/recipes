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

        [SetUp]
        public void Init()
        {
            InitRecipes();
            var recipesMock = new Mock<IRecipesService>();
            recipesMock.Setup(service => service.GetAll()).Returns(new List<Recipe>{_recipe1, _recipe2});
            var tfIdfMock = new Mock<ITfIdfService>();
            _tfIdfComputer = new TfIdfComputer(recipesMock.Object, tfIdfMock.Object);
            _tfIdfComputer = new TfIdfComputer(recipesMock.Object, tfIdfMock.Object);

        }

        private void InitRecipes()
        {
            _recipe1 = new Recipe
            {
                Directions= "\r\nRecipe test A "
            };
            _recipe2 = new Recipe();
            string readFromFile = File.ReadAllText("../../../SampleDirections/recipe2.txt");
            _recipe2.Directions = readFromFile;
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
    }
}