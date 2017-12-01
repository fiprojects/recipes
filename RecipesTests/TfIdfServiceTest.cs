using NUnit.Framework;
using RecipesCore;
using RecipesCore.Services;

namespace RecipesTests
{
    [TestFixture]
    public class TfIdfService
    {
        private ITfIdfService _tfIdfService;

        [SetUp]
        public void Init()
        {
            var db = new RecipesContext();
            _tfIdfService = new TfIdfService(db);
        }
    }
}
