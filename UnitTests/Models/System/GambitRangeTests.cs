using RedditEmblemAPI.Models.Configuration.System.Battalions;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class GambitRangeTests
    {
        [Test]
        public void Constructor_RequiredFields_Null()
        {
            GambitRangeConfig config = new GambitRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>();

            Assert.Throws<PositiveIntegerException>(() => new GambitRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInvalidMinRange()
        {
            GambitRangeConfig config = new GambitRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "-1", "0" };

            Assert.Throws<PositiveIntegerException>(() => new GambitRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInvalidMaxRange()
        {
            GambitRangeConfig config = new GambitRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "0", "-1" };

            Assert.Throws<PositiveIntegerException>(() => new GambitRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInvalidRangeSet()
        {
            GambitRangeConfig config = new GambitRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "2", "1" };

            Assert.Throws<MinimumGreaterThanMaximumException>(() => new GambitRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            GambitRangeConfig config = new GambitRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "1", "1" };

            IGambitRange range = new GambitRange(config, data);

            Assert.That(range.Minimum, Is.EqualTo(1));
            Assert.That(range.Maximum, Is.EqualTo(1));
        }
    }
}
