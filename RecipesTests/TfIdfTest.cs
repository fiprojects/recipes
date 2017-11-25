using NFluent;
using NUnit.Framework;

namespace RecipesTests
{
    [TestFixture]
    public class TfIdfTest
    {
        [Test]
        public void BasicTest()
        {
            Assert.AreEqual(15, 15);
        }

        [Test]
        public void NFluentTest()
        {
            Check.That(15).Equals(15);
        }
    }
}