using RedditEmblemAPI.Models.Configuration.System.CombatArts;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class CombatArtRangeTests
    {
        #region Constants

        private const string ART_RANGE_VAL_1 = "1";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_Null()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>();

            ICombatArtRange range = new CombatArtRange(config, data);

            Assert.That(range.Minimum, Is.EqualTo(0));
            Assert.That(range.Maximum, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_RequiredFields_WithInvalidMinRange()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "-1", "0" };

            Assert.Throws<PositiveIntegerException>(() => new CombatArtRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInvalidMaxRange()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "0", "-1" };

            Assert.Throws<PositiveIntegerException>(() => new CombatArtRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_WithInvalidRangeSet()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "2", "1" };

            Assert.Throws<MinimumGreaterThanMaximumException>(() => new CombatArtRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_MinRangeUnset()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "0", "1" };

            Assert.Throws<ItemRangeMinimumNotSetException>(() => new CombatArtRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields_MaxRangeUnset()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { "1", "0" };

            Assert.Throws<ItemRangeMinimumNotSetException>(() => new CombatArtRange(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            CombatArtRangeConfig config = new CombatArtRangeConfig()
            {
                Minimum = 0,
                Maximum = 1
            };

            IEnumerable<string> data = new List<string>() { ART_RANGE_VAL_1, ART_RANGE_VAL_1 };

            ICombatArtRange range = new CombatArtRange(config, data);

            Assert.That(range.Minimum, Is.EqualTo(1));
            Assert.That(range.Maximum, Is.EqualTo(1));
        }
    }
}
