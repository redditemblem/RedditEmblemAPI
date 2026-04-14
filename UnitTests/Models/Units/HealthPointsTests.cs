using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.Units
{
    public class HealthPointsTests
    {
        [Test]
        public void Constructor_RequiredFields_IndexOutOfBounds()
        {
            HPConfig config = new HPConfig()
            {
                Current = (0, 0),
                Maximum = (0, 1)
            };

            IEnumerable<IEnumerable<string>> data = [];

            Assert.Throws<PositiveIntegerException>(() => new HealthPoints(data, config));
        }

        [Test]
        public void Constructor_RequiredFields_InvalidInput()
        {
            HPConfig config = new HPConfig()
            {
                Current = (0, 0),
                Maximum = (0, 1)
            };

            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]
                {
                    "-1",
                    "0"
                }
            };

            Assert.Throws<PositiveIntegerException>(() => new HealthPoints(data, config));
        }

        [TestCase("150", "100", 150, 100, 150, 0)] //150% overfilled
        [TestCase("100", "100", 100, 100, 100, 0)] //full
        [TestCase("75", "100", 75, 100, 75, 25)] //75%
        [TestCase("50", "100", 50, 100, 50, 50)] //50%
        [TestCase("25", "100", 25, 100, 25, 75)] //25%
        [TestCase("0", "100", 0, 100, 0, 100)] //0%
        public void Constructor_RequiredFields_ValidInputs(string input1, string input2, int expectedCurrent, int expectedMaximum, decimal expectedPercentage, int expectedDifference)
        {
            HPConfig config = new HPConfig()
            {
                Current = (0, 0),
                Maximum = (0, 1)
            };

            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]
                {
                    input1,
                    input2
                }
            };

            IHealthPoints hp = new HealthPoints(data, config);

            Assert.That(hp, Is.Not.Null);
            Assert.That(hp.Current, Is.EqualTo(expectedCurrent));
            Assert.That(hp.Maximum, Is.EqualTo(expectedMaximum));
            Assert.That(hp.Percentage, Is.EqualTo(expectedPercentage));
            Assert.That(hp.Difference, Is.EqualTo(expectedDifference));
            Assert.That(hp.RemainingBars, Is.Zero);
        }

        #region OptionalField_RemainingBars

        [TestCase("", 0)]
        [TestCase("1", 1)]
        public void Constructor_OptionalField_RemainingBars_ValidInputs(string input, int expected)
        {
            HPConfig config = new HPConfig()
            {
                Current = (0, 0),
                Maximum = (0, 1),
                RemainingBars = (0, 2)
            };

            IEnumerable<IEnumerable<string>> data = new string[][]
            {
                new string[]
                {
                    "20",
                    "20",
                    input
                }
            };

            IHealthPoints hp = new HealthPoints(data, config);

            Assert.That(hp.RemainingBars, Is.EqualTo(expected));
        }

        #endregion OptionalField_RemainingBars
    }
}
