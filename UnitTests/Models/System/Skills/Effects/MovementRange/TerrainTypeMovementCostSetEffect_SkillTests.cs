
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;

namespace UnitTests.Models.System.Skills.Effects.MovementRange
{
    public class TerrainTypeMovementCostSetEffect_SkillTests
    {
        #region Constructor

        [Test]
        public void Constructor_EmptyInput()
        {
            IEnumerable<string> data = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new TerrainTypeMovementCostSetEffect_Skill(data));
        }

        [Test]
        public void Constructor_EmptyString()
        {
            IEnumerable<string> data = new List<string>()
            {
                string.Empty
            };

            Assert.Throws<SkillEffectMissingParameterException>(() => new TerrainTypeMovementCostSetEffect_Skill(data));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> data = new List<string>()
            {
                string.Empty,
                string.Empty
            };

            Assert.Throws<SkillEffectMissingParameterException>(() => new TerrainTypeMovementCostSetEffect_Skill(data));
        }

        [Test]
        public void Constructor_3EmptyStrings()
        {
            IEnumerable<string> data = new List<string>()
            {
                string.Empty,
                string.Empty,
                string.Empty
            };

            Assert.Throws<PositiveIntegerException>(() => new TerrainTypeMovementCostSetEffect_Skill(data));
        }

        [TestCase("No", false)]
        [TestCase("Yes", true)]
        public void Constructor(string canOverride99MoveCost, bool expected)
        {
            IEnumerable<string> data = new List<string>()
            {
                "2",
                "3",
                canOverride99MoveCost
            };

            ITerrainTypeMovementCostSetEffect_Skill effect = new TerrainTypeMovementCostSetEffect_Skill(data);

            Assert.That(effect.TerrainTypeGrouping, Is.EqualTo(2));
            Assert.That(effect.Value, Is.EqualTo(3));
            Assert.That(effect.CanOverride99MoveCost, Is.EqualTo(expected));
        }

        #endregion Constructor
    }
}
