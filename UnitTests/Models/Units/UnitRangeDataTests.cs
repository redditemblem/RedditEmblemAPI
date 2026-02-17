using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.Units
{
    public class UnitRangeDataTests
    {
        [Test]
        public void Constructor()
        {
            IUnitRangeData ranges = new UnitRangeData();

            Assert.That(ranges, Is.Not.Null);
            Assert.That(ranges.Movement, Is.Empty);
            Assert.That(ranges.Attack, Is.Empty);
            Assert.That(ranges.Utility, Is.Empty);
        }
    }
}
