using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Configuration.System.WeaponRankBonuses;

namespace UnitTests.Models.Configuration.Common
{
    [TestClass]
    public class ExtensionMethodsTests
    {
        #region AddQueryable

        [TestMethod]
        public void ExtensionMethods_AddQueryable_IQueryable_NullQuery()
        {
            List<Query> queries = new List<Query>();
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig();

            bool wasAdded = queries.AddQueryable(config);

            Assert.IsFalse(wasAdded);
            Assert.AreEqual(0, queries.Count);
        }

        [TestMethod]
        public void ExtensionMethods_AddQueryable_IQueryable()
        {
            List<Query> queries = new List<Query>();
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Query = new Query(),
                Category = 0
            };

            bool wasAdded = queries.AddQueryable(config);

            Assert.IsTrue(wasAdded);
            Assert.AreEqual(1, queries.Count);
        }

        [TestMethod]
        public void ExtensionMethods_AddQueryable_IMultiQueryable_NullQuery()
        {
            List<Query> queries = new List<Query>();
            AffiliationsConfig config = new AffiliationsConfig();

            bool wasAdded = queries.AddQueryable(config);

            Assert.IsFalse(wasAdded);
            Assert.AreEqual(0, queries.Count);
        }

        [TestMethod]
        public void ExtensionMethods_AddQueryable_IMultiQueryable_EmptyList()
        {
            List<Query> queries = new List<Query>();
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<Query>(),
                Name = 0,
                Grouping = 1
            };

            bool wasAdded = queries.AddQueryable(config);

            Assert.IsFalse(wasAdded);
            Assert.AreEqual(0, queries.Count);
        }

        [TestMethod]
        public void ExtensionMethods_AddQueryable_IMultiQueryable_SingleQuery()
        {
            List<Query> queries = new List<Query>();
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                },
                Name = 0,
                Grouping = 1
            };

            bool wasAdded = queries.AddQueryable(config);

            Assert.IsTrue(wasAdded);
            Assert.AreEqual(1, queries.Count);
        }

        [TestMethod]
        public void ExtensionMethods_AddQueryable_IMultiQueryable_MultipleQueries()
        {
            List<Query> queries = new List<Query>();
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query(),
                    new Query()
                },
                Name = 0,
                Grouping = 1
            };

            bool wasAdded = queries.AddQueryable(config);

            Assert.IsTrue(wasAdded);
            Assert.AreEqual(2, queries.Count);
        }

        #endregion AddQueryable
    }
}
