using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Configuration.System.Tags;

namespace UnitTests.Models.Configuration.Common
{
    [TestClass]
    public class ExtensionMethodsTests
    {
        #region AddQueryable

        [TestMethod]
        public void ExtensionMethods_AddQueryable_Null() 
        {
            List<Query> queries = new List<Query>();

            bool wasAdded = queries.AddQueryable(null);

            Assert.IsFalse(wasAdded);
            Assert.AreEqual(0, queries.Count);
        }

        [TestMethod]
        public void ExtensionMethods_AddQueryable_NullQuery()
        {
            List<Query> queries = new List<Query>();
            AffiliationsConfig config = new AffiliationsConfig();

            bool wasAdded = queries.AddQueryable(config);

            Assert.IsFalse(wasAdded);
            Assert.AreEqual(0, queries.Count);
        }

        [TestMethod]
        public void ExtensionMethods_AddQueryable()
        {
            List<Query> queries = new List<Query>();
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Query = new Query(),
                Name = 0,
                Grouping = 1
            };

            bool wasAdded = queries.AddQueryable(config);

            Assert.IsTrue(wasAdded);
            Assert.AreEqual(1, queries.Count);
        }

        #endregion AddQueryable
    }
}
