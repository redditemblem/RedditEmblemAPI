using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    [TestClass]
    public class OverrideMovementEffectTests
    {
        #region Constructor

        [TestMethod]
        public void OverrideMovementEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<StatusConditionEffectMissingParameterException>(() => new OverrideMovementEffect(parameters));
        }

        [TestMethod]
        public void OverrideMovementEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<PositiveIntegerException>(() => new OverrideMovementEffect(parameters));
        }

        [TestMethod]
        public void OverrideMovementEffect_Constructor_InvalidMovementValue()
        {
            List<string> parameters = new List<string>() { "-1" };

            Assert.ThrowsException<PositiveIntegerException>(() => new OverrideMovementEffect(parameters));
        }

        [TestMethod]
        public void OverrideMovementEffect_Constructor_MovementValue_Zero()
        {
            List<string> parameters = new List<string>() { "0" };
            OverrideMovementEffect effect = new OverrideMovementEffect(parameters);

            Assert.AreEqual<int>(0, effect.MovementValue);
        }

        [TestMethod]
        public void OverrideMovementEffect_Constructor_MovementValue_One()
        {
            List<string> parameters = new List<string>() { "1" };
            OverrideMovementEffect effect = new OverrideMovementEffect(parameters);

            Assert.AreEqual<int>(1, effect.MovementValue);
        }

        #endregion Constructor
    }
}