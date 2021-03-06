﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RecipesCore;
using RecipesCore.Models;
using RecipesCore.Services;

namespace RecipesTests
{
    [TestFixture]
    public class TfIdfServiceTest
    {
        private ITfIdfService _tfIdfService;

        private TfIdfModel _model1;
        
        private TfIdfModel _model2;

        private TfIdfModel _model3;

        private TfIdfModel _model4;
        
        private double _threshold = 1e-10;
        private Recipe _recipe1;
        private Recipe _recipe2;
        private Recipe _recipe3;
        private Recipe _recipe4;

        // this method is inspired by https://stackoverflow.com/questions/34332761/mocking-dbsett-inline
        private DbSet<T> ToDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);
            return dbSet.Object;
        }
        
        [SetUp]
        public void Init()
        {
            InitRecipes();
            InitTfIdfModels();
            var modelsList = new List<TfIdfModel> {_model1, _model2, _model3, _model4};
            var mock = new Mock<RecipesContext>();
            mock.Setup(context => context.TfIdfModels).Returns(() => ToDbSet<TfIdfModel>(modelsList));
            _tfIdfService = new TfIdfService(mock.Object);
        }

        private void InitRecipes()
        {
            _recipe1 = new Recipe
            {
                Id = 1,
                Description = "basil soup"
            };
            _recipe2 = new Recipe
            {
                Id = 2,
                Description = "egg"
            };
            _recipe3 = new Recipe
            {
                Id = 3,
                Description = "soup with egg and basil"
            };
            _recipe4 = new Recipe
            {
                Id = 4,
                Description = "egg on bread"
            };
        }

        private void InitTfIdfModels()
        {
            _model1 = new TfIdfModel
            {
                Recipe = _recipe1
            };
            _model1.Elements.Add(new TfIdfElement{Term = "a", TfIdf = 0.054});
            _model1.Elements.Add(new TfIdfElement{Term = "soup", TfIdf = 0.103});
            _model1.Elements.Add(new TfIdfElement{Term = "with", TfIdf = 0.004});
            _model1.Elements.Add(new TfIdfElement{Term = "basil", TfIdf = 1.054});
            
            _model2 = new TfIdfModel
            {
                Recipe = _recipe2
            };
            _model2.Elements.Add(new TfIdfElement{Term = "egg", TfIdf = 2.4});
            _model2.Elements.Add(new TfIdfElement{Term = "with", TfIdf = 0.004});
            _model2.Elements.Add(new TfIdfElement{Term = "pepper", TfIdf = 0.72});
            _model2.Elements.Add(new TfIdfElement{Term = "and", TfIdf = 0.005});
            _model2.Elements.Add(new TfIdfElement{Term = "salt", TfIdf = 1.0456789});
            
            _model3 = new TfIdfModel
            {
                Recipe = _recipe3
            };
            _model3.Elements.Add(new TfIdfElement{Term = "mix", TfIdf = 1.32});
            _model3.Elements.Add(new TfIdfElement{Term = "egg", TfIdf = 2.5});
            _model3.Elements.Add(new TfIdfElement{Term = "soup", TfIdf = 0.42});
            _model3.Elements.Add(new TfIdfElement{Term = "and", TfIdf = 0.0048});
            _model3.Elements.Add(new TfIdfElement{Term = "basil", TfIdf = 1.278424});

            _model4 = new TfIdfModel
            {
                Recipe = _recipe4
            };
            _model4.Elements.Add(new TfIdfElement{Term = "egg", TfIdf = 2.7});
            _model4.Elements.Add(new TfIdfElement{Term = "with", TfIdf = 0.0035});
            _model4.Elements.Add(new TfIdfElement{Term = "bread", TfIdf = 1.72});
            _model4.Elements.Add(new TfIdfElement{Term = "and", TfIdf = 0.00034});
            _model4.Elements.Add(new TfIdfElement{Term = "salt", TfIdf = 1.0456789});
        }

        [Test]
        public void GetAll()
        {
            var getAllList = new List<TfIdfModel> {_model1, _model2, _model3, _model4};
            var ret = _tfIdfService.GetAll();
            CollectionAssert.AreEquivalent(getAllList, ret);
        }
        
        [Test]
        public void GetAllExceptTest()
        {
            var expectedList = new List<TfIdfModel>{_model1, _model3, _model4};
            var retList = _tfIdfService.GetAllExcept(_model2);
            CollectionAssert.AreEquivalent(expectedList, retList);
        }

        [Test]
        public void ComputeZeroSimilarityTest()
        {
            var elem = new TfIdfModel
            {
                Recipe = new Recipe()
            };
            double similarity = _tfIdfService.ComputeCosineSimilarity(elem, _model1);
            double expectedSim = -1.0f;
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }
        
        [Test]
        public void ComputeSimilarityBetweenOneAndTwoTest()
        {
            double expectedSim = 0.000016 / (1.060404168 * 2.715121611);  // 0,000005557
            var similarity = _tfIdfService.ComputeCosineSimilarity(_model1, _model2);
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }
        
        [Test]
        public void ComputeSimilarityBetweenOneAndThreeTest()
        {
            double expectedSim = 1.390718896 / (1.060404168 * 3.131004785);  // 0,418874766
            var similarity = _tfIdfService.ComputeCosineSimilarity(_model1, _model3);
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }

        [Test]
        public void ComputeSimilarityBetweenOneAndFourTest()
        {
            double expectedSim = 0.000014 / (1.060404168 * 3.367767321);  // 0,00000392
            var similarity = _tfIdfService.ComputeCosineSimilarity(_model1, _model4);
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }
        
        [Test]
        public void ComputeSimilarityBetweenTwoAndThreeTest()
        {
            double expectedSim = 6.000024 / (2.715121611 * 3.131004785);  // 0,705797263
            var similarity = _tfIdfService.ComputeCosineSimilarity(_model2, _model3);
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }

        [Test]
        public void ComputeSimilarityBetweenTwoAndFourTest()
        {
            double expectedSim = 7.573460062 / (2.715121611 * 3.367767321);  // 0,828252918
            var similarity = _tfIdfService.ComputeCosineSimilarity(_model2, _model4);
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }
        
        [Test]
        public void ComputeSimilarityBetweenThreeAndFourTest()
        {
            double expectedSim = 6.750001632 / (3.131004785 * 3.367767321);  // 0,640144573
            var similarity = _tfIdfService.ComputeCosineSimilarity(_model3, _model4);
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }
        
        [Test]
        public void ComputeSimilarityWithoutOrderMeaningTest()
        {
            double abSimilarity = _tfIdfService.ComputeCosineSimilarity(_model1, _model3);
            double baSimilarity = _tfIdfService.ComputeCosineSimilarity(_model3, _model1);
            Assert.AreEqual(abSimilarity, baSimilarity);
        }

        [Test]
        public void GetSimilarRecipesForModelOneTest()
        {
            var rankedList = _tfIdfService.GetSimilarRecipesForModel(_model1);
            MyCompareRecipes(_recipe3, rankedList[0]);
            MyCompareRecipes(_recipe2, rankedList[1]);
            MyCompareRecipes(_recipe4, rankedList[2]);
        }

        [Test]
        public void GetSimilarRecipesForModelTwoTest()
        {
            var rankedList = _tfIdfService.GetSimilarRecipesForModel(_model2);
            MyCompareRecipes(_recipe4, rankedList[0]);
            MyCompareRecipes(_recipe3, rankedList[1]);
            MyCompareRecipes(_recipe1, rankedList[2]);
        }
        
        [Test]
        public void GetSimilarRecipesForModelThreeTest()
        {
            var rankedList = _tfIdfService.GetSimilarRecipesForModel(_model3);
            MyCompareRecipes(_recipe2, rankedList[0]);
            MyCompareRecipes(_recipe4, rankedList[1]);
            MyCompareRecipes(_recipe1, rankedList[2]);
        }
        
        [Test]
        public void GetSimilarRecipesForModelFourTest()
        {
            var rankedList = _tfIdfService.GetSimilarRecipesForModel(_model4);
            MyCompareRecipes(_recipe2, rankedList[0]);
            MyCompareRecipes(_recipe3, rankedList[1]);
            MyCompareRecipes(_recipe1, rankedList[2]);
        }

        [Test]
        public void GetSimilarRecipesForRecipeTwoTest()
        {
            var rankedList = _tfIdfService.GetSimilarRecipesForRecipe(_recipe2);
            MyCompareRecipes(_recipe4, rankedList[0]);
            MyCompareRecipes(_recipe3, rankedList[1]);
            MyCompareRecipes(_recipe1, rankedList[2]);
        }
        
        private void MyCompareRecipes(Recipe expected, Recipe returned)
        {
            Assert.AreEqual(expected.Id, returned.Id);
            Assert.AreEqual(expected.Description, returned.Description);
        }
    }
}
