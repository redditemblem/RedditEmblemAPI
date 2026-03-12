using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    public class OverrideMovementEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new OverrideMovementEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<PositiveIntegerException>(() => new OverrideMovementEffect(parameters));
        }

        [Test]
        public void Constructor_InvalidMovementValue()
        {
            IEnumerable<string> parameters = new List<string>() { "-1" };

            Assert.Throws<PositiveIntegerException>(() => new OverrideMovementEffect(parameters));
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        public void Constructor(string value, int expected)
        {
            IEnumerable<string> parameters = new List<string>() { value };
            OverrideMovementEffect effect = new OverrideMovementEffect(parameters);

            Assert.That(effect.MovementValue, Is.EqualTo(expected));
        }

        #endregion Constructor
    }
}