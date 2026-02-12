using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    public class OverrideMovementTypeEffect_StatusTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new OverrideMovementTypeEffect_Status(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new OverrideMovementTypeEffect_Status(parameters));
        }

        [Test]
        public void Constructor()
        {
            List<string> parameters = new List<string>() { "MoveType" };
            OverrideMovementTypeEffect_Status effect = new OverrideMovementTypeEffect_Status(parameters);

            Assert.That(effect.MovementType, Is.EqualTo("MoveType"));
        }

        #endregion Constructor
    }
}