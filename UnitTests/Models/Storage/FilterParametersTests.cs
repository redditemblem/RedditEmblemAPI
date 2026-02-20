using NSubstitute;
using RedditEmblemAPI.Models.Output.Storage;

namespace UnitTests.Models.Storage
{
    public class FilterParametersTests
    {
        [Test]
        public void Constructor()
        {
            List<IItemSort> itemSorts = new List<IItemSort>()
            {
                Substitute.For<IItemSort>()
            };
            IEnumerable<string> owners = new List<string>() { "IronPegasus" };
            IEnumerable<string> itemCategories = new List<string>() { "Sword" };
            IEnumerable<string> utilizedStats = new List<string>() { "Str" };
            IEnumerable<string> targetedStats = new List<string>() { "Def" };
            IDictionary<string, bool> filterConditions = new Dictionary<string, bool>();
            filterConditions.Add("test", false);

            IFilterParameters parms = new FilterParameters(itemSorts, owners, itemCategories, utilizedStats, targetedStats, filterConditions);

            Assert.That(parms, Is.Not.Null);
            Assert.That(parms.Sorts, Is.EqualTo(itemSorts));
            Assert.That(parms.Owners, Is.EqualTo(owners));
            Assert.That(parms.ItemCategories, Is.EqualTo(itemCategories));
            Assert.That(parms.UtilizedStats, Is.EqualTo(utilizedStats));
            Assert.That(parms.TargetedStats, Is.EqualTo(targetedStats));
            Assert.That(parms.FilterConditions, Is.EqualTo(filterConditions));
        }
    }
}
