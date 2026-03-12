using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;

namespace UnitTests.Models.System.Skills.Effects.MovementRange
{
    public class WarpMovementCostModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_EmptyInput()
        {
            IEnumerable<string> data = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new WarpMovementCostModifierEffect(data));
        }

        [Test]
        public void Constructor_EmptyString()
        {
            IEnumerable<string> data = new List<string>()
            {
                string.Empty
            };

            Assert.Throws<SkillEffectMissingParameterException>(() => new WarpMovementCostModifierEffect(data));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> data = new List<string>()
            {
                string.Empty,
                string.Empty
            };

            Assert.Throws<PositiveIntegerException>(() => new WarpMovementCostModifierEffect(data));
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<string> data = new List<string>()
            {
                "2",
                "-1"
            };

            WarpMovementCostModifierEffect effect = new WarpMovementCostModifierEffect(data);

            Assert.That(effect.TerrainTypeGrouping, Is.EqualTo(2));
            Assert.That(effect.Value, Is.EqualTo(-1));
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.Standard));
        }

        #endregion Constructor
    }
}
