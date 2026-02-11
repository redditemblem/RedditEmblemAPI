using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Configuration.System.WeaponRankBonuses;

namespace UnitTests.Models.Configuration.Common
{
    public class ExtensionMethodsTests
    {
        #region AddQueryable

        [Test]
        public void ExtensionMethods_AddQueryable_IQueryable_NullQuery()
        {
            List<IQuery> queries = new List<IQuery>();
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig();

            bool wasAdded = queries.AddQueryable(config);

            Assert.That(wasAdded, Is.False);
            Assert.That(queries, Is.Empty);
        }

        [Test]
        public void ExtensionMethods_AddQueryable_IQueryable()
        {
            List<IQuery> queries = new List<IQuery>();
            WeaponRankBonusesConfig config = new WeaponRankBonusesConfig()
            {
                Query = new Query(),
                Category = 0
            };

            bool wasAdded = queries.AddQueryable(config);

            Assert.That(wasAdded, Is.True);
            Assert.That(queries.Count, Is.EqualTo(1));
        }

        [Test]
        public void ExtensionMethods_AddQueryable_IMultiQueryable_NullQuery()
        {
            List<IQuery> queries = new List<IQuery>();
            AffiliationsConfig config = new AffiliationsConfig();

            bool wasAdded = queries.AddQueryable(config);

            Assert.That(wasAdded, Is.False);
            Assert.That(queries, Is.Empty);
        }

        [Test]
        public void ExtensionMethods_AddQueryable_IMultiQueryable_EmptyList()
        {
            List<IQuery> queries = new List<IQuery>();
            AffiliationsConfig config = new AffiliationsConfig()
            {
                Queries = new List<Query>(),
                Name = 0,
                Grouping = 1
            };

            bool wasAdded = queries.AddQueryable(config);

            Assert.That(wasAdded, Is.False);
            Assert.That(queries, Is.Empty);
        }

        [Test]
        public void ExtensionMethods_AddQueryable_IMultiQueryable_SingleQuery()
        {
            List<IQuery> queries = new List<IQuery>();
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

            Assert.That(wasAdded, Is.True);
            Assert.That(queries.Count, Is.EqualTo(1));
        }

        [Test]
        public void ExtensionMethods_AddQueryable_IMultiQueryable_MultipleQueries()
        {
            List<IQuery> queries = new List<IQuery>();
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

            Assert.That(wasAdded, Is.True);
            Assert.That(queries.Count, Is.EqualTo(2));
        }

        #endregion AddQueryable
    }
}
