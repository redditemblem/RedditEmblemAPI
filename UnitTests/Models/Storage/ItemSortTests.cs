using RedditEmblemAPI.Models.Output.Storage;

namespace UnitTests.Models.Storage
{
    public class ItemSortTests
    {
        [Test]
        public void Constructor()
        {
            string displayName = "Name";
            string sortAttribute = "name";
            bool isDeepSort = true;

            IItemSort sort = new ItemSort(displayName, sortAttribute, isDeepSort);

            Assert.That(sort, Is.Not.Null);
            Assert.That(sort.DisplayName, Is.EqualTo(displayName));
            Assert.That(sort.SortAttribute, Is.EqualTo(sortAttribute));
            Assert.That(sort.IsDeepSort, Is.True);
        }
    }
}
