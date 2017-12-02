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
            InitTfIdfModels();
            var modelsList = new List<TfIdfModel> {_model1, _model2, _model3};
            var mock = new Mock<RecipesContext>();
            mock.Setup(context => context.TfIdfModels).Returns(() => ToDbSet<TfIdfModel>(modelsList));
            _tfIdfService = new TfIdfService(mock.Object);
        }

        private void InitTfIdfModels()
        {
            _model1 = new TfIdfModel
            {
                Recipe = new Recipe()
            };
            _model1.Elements.Add(new TfIdfElement{Term = "a", TfIdf = 0.054});
            _model1.Elements.Add(new TfIdfElement{Term = "soup", TfIdf = 0.103});
            _model1.Elements.Add(new TfIdfElement{Term = "with", TfIdf = 0.004});
            _model1.Elements.Add(new TfIdfElement{Term = "basil", TfIdf = 1.054});
            
            _model2 = new TfIdfModel
            {
                Recipe = new Recipe()
            };
            _model2.Elements.Add(new TfIdfElement{Term = "egg", TfIdf = 2.4});
            _model2.Elements.Add(new TfIdfElement{Term = "with", TfIdf = 0.004});
            _model2.Elements.Add(new TfIdfElement{Term = "pepper", TfIdf = 0.72});
            _model2.Elements.Add(new TfIdfElement{Term = "and", TfIdf = 0.005});
            _model2.Elements.Add(new TfIdfElement{Term = "salt", TfIdf = 1.0456789});
            
            _model3 = new TfIdfModel
            {
                Recipe = new Recipe()
            };
            _model3.Elements.Add(new TfIdfElement{Term = "mix", TfIdf = 1.32});
            _model3.Elements.Add(new TfIdfElement{Term = "egg", TfIdf = 2.5});
            _model3.Elements.Add(new TfIdfElement{Term = "soup", TfIdf = 0.42});
            _model3.Elements.Add(new TfIdfElement{Term = "and", TfIdf = 0.0048});
            _model3.Elements.Add(new TfIdfElement{Term = "basil", TfIdf = 1.278424});
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
    }
}
