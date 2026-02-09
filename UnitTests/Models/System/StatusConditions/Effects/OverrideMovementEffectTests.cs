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
            List<string> parameters = new List<string>();

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new OverrideMovementEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<PositiveIntegerException>(() => new OverrideMovementEffect(parameters));
        }

        [Test]
        public void Constructor_InvalidMovementValue()
        {
            List<string> parameters = new List<string>() { "-1" };

            Assert.Throws<PositiveIntegerException>(() => new OverrideMovementEffect(parameters));
        }

        [Test]
        public void Constructor_MovementValue_Zero()
        {
            List<string> parameters = new List<string>() { "0" };
            OverrideMovementEffect effect = new OverrideMovementEffect(parameters);

            Assert.That(effect.MovementValue, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_MovementValue_One()
        {
            List<string> parameters = new List<string>() { "1" };
            OverrideMovementEffect effect = new OverrideMovementEffect(parameters);

            Assert.That(effect.MovementValue, Is.EqualTo(1));
        }

        #endregion Constructor
    }
}