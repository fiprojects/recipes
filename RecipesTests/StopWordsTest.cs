using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using RecipesCore.Processors;
using Assert = NUnit.Framework.Assert;

namespace RecipesTests
{
    [TestFixture]
    public class StopWordsTest
    {
        private StopWords _stopWords;

        [SetUp]
        public void Init()
        {
            _stopWords = new StopWords("../../../../Recipes/stop_words.json");
        }

        [Test]
        public void LoadNonExistingFileTest()
        {
            Assert.Throws<FileNotFoundException>(() => new StopWords("/"));
        }
        
        [Test]
        public void IsStopWordTest()
        {
            var ret = _stopWords.IsStopWord("and");
            Assert.True(ret);
        }

        [Test]
        public void IsllStopWordTest()
        {
            var ret = _stopWords.IsStopWord("'ll");
            Assert.True(ret);
        }
        [Test]
        public void IsNotStopWordTest()
        {
            var ret = _stopWords.IsStopWord("recipe");
            Assert.False(ret);
        }
    }
}