using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    [TestClass]
    public class OverrideMovementTypeEffect_StatusTests
    {
        #region Constructor

        [TestMethod]
        public void OverrideMovementTypeEffect_Status_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<StatusConditionEffectMissingParameterException>(() => new OverrideMovementTypeEffect_Status(parameters));
        }

        [TestMethod]
        public void OverrideMovementTypeEffect_Status_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new OverrideMovementTypeEffect_Status(parameters));
        }

        [TestMethod]
        public void OverrideMovementTypeEffect_Status_Constructor()
        {
            List<string> parameters = new List<string>() { "MoveType" };
            OverrideMovementTypeEffect_Status effect = new OverrideMovementTypeEffect_Status(parameters);

            Assert.AreEqual<string>("MoveType", effect.MovementType);
        }

        #endregion Constructor
    }
}