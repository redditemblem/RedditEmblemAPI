using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    [TestClass]
    public class TerrainTypeMovementCostSetEffect_StatusTests
    {
        #region Constructor

        [TestMethod]
        public void TerrainTypeMovementCostSetEffect_Status_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<StatusConditionEffectMissingParameterException>(() => new TerrainTypeMovementCostSetEffect_Status(parameters));
        }

        [TestMethod]
        public void TerrainTypeMovementCostSetEffect_Status_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<StatusConditionEffectMissingParameterException>(() => new TerrainTypeMovementCostSetEffect_Status(parameters));
        }

        [TestMethod]
        public void TerrainTypeMovementCostSetEffect_Status_Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.ThrowsException<StatusConditionEffectMissingParameterException>(() => new TerrainTypeMovementCostSetEffect_Status(parameters));
        }

        [TestMethod]
        public void TerrainTypeMovementCostSetEffect_Status_Constructor_3EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.ThrowsException<PositiveIntegerException>(() => new TerrainTypeMovementCostSetEffect_Status(parameters));
        }

        [TestMethod]
        public void TerrainTypeMovementCostSetEffect_Status_Constructor_TerrainTypeGrouping_Neg1()
        {
            List<string> parameters = new List<string>() { "-1", string.Empty, string.Empty };

            Assert.ThrowsException<PositiveIntegerException>(() => new TerrainTypeMovementCostSetEffect_Status(parameters));
        }

        [TestMethod]
        public void TerrainTypeMovementCostSetEffect_Status_Constructor_Value_Neg1()
        {
            List<string> parameters = new List<string>() { "0", "-1", string.Empty };

            Assert.ThrowsException<PositiveIntegerException>(() => new TerrainTypeMovementCostSetEffect_Status(parameters));
        }

        [TestMethod]
        public void TerrainTypeMovementCostSetEffect_Status_Constructor_EmptyCanOverride99MoveCost()
        {
            List<string> parameters = new List<string>() { "0", "0", string.Empty };

            TerrainTypeMovementCostSetEffect_Status effect = new TerrainTypeMovementCostSetEffect_Status(parameters);
            Assert.AreEqual<int>(0, effect.TerrainTypeGrouping);
            Assert.AreEqual<int>(0, effect.Value);
            Assert.IsFalse(effect.CanOverride99MoveCost);
        }

        [TestMethod]
        public void TerrainTypeMovementCostSetEffect_Status_Constructor_CanOverride99MoveCost_No()
        {
            List<string> parameters = new List<string>() { "0", "0", "No" };

            TerrainTypeMovementCostSetEffect_Status effect = new TerrainTypeMovementCostSetEffect_Status(parameters);
            Assert.AreEqual<int>(0, effect.TerrainTypeGrouping);
            Assert.AreEqual<int>(0, effect.Value);
            Assert.IsFalse(effect.CanOverride99MoveCost);
        }

        [TestMethod]
        public void TerrainTypeMovementCostSetEffect_Status_Constructor_CanOverride99MoveCost_Yes()
        {
            List<string> parameters = new List<string>() { "1", "1", "Yes" };

            TerrainTypeMovementCostSetEffect_Status effect = new TerrainTypeMovementCostSetEffect_Status(parameters);
            Assert.AreEqual<int>(1, effect.TerrainTypeGrouping);
            Assert.AreEqual<int>(1, effect.Value);
            Assert.IsTrue(effect.CanOverride99MoveCost);
        }

        #endregion Constructor
    }
}
