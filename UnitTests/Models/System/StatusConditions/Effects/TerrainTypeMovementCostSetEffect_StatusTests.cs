using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    public class TerrainTypeMovementCostSetEffect_StatusTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new TerrainTypeMovementCostSetEffect_Status(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new TerrainTypeMovementCostSetEffect_Status(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new TerrainTypeMovementCostSetEffect_Status(parameters));
        }

        [Test]
        public void Constructor_3EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.Throws<PositiveIntegerException>(() => new TerrainTypeMovementCostSetEffect_Status(parameters));
        }

        [Test]
        public void Constructor_TerrainTypeGrouping_Neg1()
        {
            List<string> parameters = new List<string>() { "-1", string.Empty, string.Empty };

            Assert.Throws<PositiveIntegerException>(() => new TerrainTypeMovementCostSetEffect_Status(parameters));
        }

        [Test]
        public void Constructor_Value_Neg1()
        {
            List<string> parameters = new List<string>() { "0", "-1", string.Empty };

            Assert.Throws<PositiveIntegerException>(() => new TerrainTypeMovementCostSetEffect_Status(parameters));
        }

        [Test]
        public void Constructor_EmptyCanOverride99MoveCost()
        {
            List<string> parameters = new List<string>() { "0", "0", string.Empty };

            TerrainTypeMovementCostSetEffect_Status effect = new TerrainTypeMovementCostSetEffect_Status(parameters);
            Assert.That(effect.TerrainTypeGrouping, Is.EqualTo(0));
            Assert.That(effect.Value, Is.EqualTo(0));
            Assert.That(effect.CanOverride99MoveCost, Is.False);
        }

        [Test]
        public void Constructor_CanOverride99MoveCost_No()
        {
            List<string> parameters = new List<string>() { "0", "0", "No" };

            TerrainTypeMovementCostSetEffect_Status effect = new TerrainTypeMovementCostSetEffect_Status(parameters);
            Assert.That(effect.TerrainTypeGrouping, Is.EqualTo(0));
            Assert.That(effect.Value, Is.EqualTo(0));
            Assert.That(effect.CanOverride99MoveCost, Is.False);
        }

        [Test]
        public void Constructor_CanOverride99MoveCost_Yes()
        {
            List<string> parameters = new List<string>() { "1", "1", "Yes" };

            TerrainTypeMovementCostSetEffect_Status effect = new TerrainTypeMovementCostSetEffect_Status(parameters);
            Assert.That(effect.TerrainTypeGrouping, Is.EqualTo(1));
            Assert.That(effect.Value, Is.EqualTo(1));
            Assert.That(effect.CanOverride99MoveCost, Is.True);
        }

        #endregion Constructor
    }
}
