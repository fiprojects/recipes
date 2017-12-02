using System.Collections.Generic;
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
            var modelsList = new List<TfIdfModel> {_model1, _model2, _model3};
            var mock = new Mock<RecipesContext>();
            mock.Setup(context => context.TfIdfModels).Returns(() => ToDbSet<TfIdfModel>(modelsList));
            _tfIdfService = new TfIdfService(mock.Object);
        }

        private void InitRecipes()
        {
            _recipe1 = new Recipe
            {
                Description = "basil soup"
            };
            _recipe2 = new Recipe
            {
                Description = "egg"
            };
            _recipe3 = new Recipe
            {
                Description = "soup with egg and basil"
            };
            _recipe4 = new Recipe
            {
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
            var getAllList = new List<TfIdfModel> {_model1, _model2, _model3};
            var ret = _tfIdfService.GetAll();
            CollectionAssert.AreEquivalent(getAllList, ret);
        }
        
        [Test]
        public void GetAllExceptTest()
        {
            var expectedList = new List<TfIdfModel>{_model1, _model3};
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
            double expectedSim = 0.0f;
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }
        
        [Test]
        public void ComputeSimilarityBetweenOneAndTwoTest()
        {
            double expectedSim = 1.0;
            var similarity = _tfIdfService.ComputeCosineSimilarity(_model1, _model2);
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }
        
        [Test]
        public void ComputeSimilarityBetweenOneAndThreeTest()
        {
            double expectedSim = 0.9758958692;
            var similarity = _tfIdfService.ComputeCosineSimilarity(_model1, _model3);
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }

        [Test]
        public void ComputeSimilarityBetweenOneAndFourTest()
        {
            double expectedSim = 1.0;
            var similarity = _tfIdfService.ComputeCosineSimilarity(_model1, _model4);
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }
        
        [Test]
        public void ComputeSimilarityBetweenTwoAndThreeTest()
        {
            double expectedSim = 0.9999999867;
            var similarity = _tfIdfService.ComputeCosineSimilarity(_model2, _model3);
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }

        [Test]
        public void ComputeSimilarityBetweenTwoAndFourTest()
        {
            double expectedSim = 0.9991415764;
            var similarity = _tfIdfService.ComputeCosineSimilarity(_model2, _model4);
            Assert.That(similarity, Is.EqualTo(expectedSim).Within(_threshold));
        }
        
        [Test]
        public void ComputeSimilarityBetweenThreeAndFourTest()
        {
            double expectedSim = 0.9999983907;
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
    }
}
