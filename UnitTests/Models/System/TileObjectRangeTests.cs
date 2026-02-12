using RedditEmblemAPI.Models.Configuration.System.TileObjects;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class TileObjectRangeTests
    {
        [Test]
        public void Constructor_RequiredFields_Null()
        {
            TileObjectRangeConfig config = new TileObjectRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>();

            ITileObjectRange range = new TileObjectRange(config, data);

            Assert.That(range.Minimum, Is.EqualTo(0));
            Assert.That(range.Maximum, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_RequiredFields_WithInvalidMinRange()
        {
            TileObjectRangeConfig config = new TileObjectRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "-1", "0" };

            Assert.Throws<PositiveIntegerException>(() => new TileObjectRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInvalidMaxRange()
        {
            TileObjectRangeConfig config = new TileObjectRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "0", "-1" };

            Assert.Throws<PositiveIntegerException>(() => new TileObjectRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInvalidRangeSet()
        {
            TileObjectRangeConfig config = new TileObjectRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "2", "1" };

            Assert.Throws<MinimumGreaterThanMaximumException>(() => new TileObjectRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            TileObjectRangeConfig config = new TileObjectRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "1", "1" };

            ITileObjectRange range = new TileObjectRange(config, data);

            Assert.That(range.Minimum, Is.EqualTo(1));
            Assert.That(range.Maximum, Is.EqualTo(1));
        }
    }
}
