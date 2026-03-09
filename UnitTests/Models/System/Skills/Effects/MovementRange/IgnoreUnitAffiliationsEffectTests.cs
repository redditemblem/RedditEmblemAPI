using NSubstitute;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.Skills.Effects.MovementRange
{
    public class IgnoreUnitAffiliationsEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor()
        {
            IEnumerable<string> data = new List<string>();
            IgnoreUnitAffiliationsEffect effect = new IgnoreUnitAffiliationsEffect(data);

            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.Standard));
        }

        #endregion Constructor

        #region IsActive

        [Test]
        public void IsActive()
        {
            IUnit unit = Substitute.For<IUnit>();
            IEnumerable<string> data = new List<string>();

            IIgnoreUnitAffiliations effect = new IgnoreUnitAffiliationsEffect(data);
            bool isActive = effect.IsActive(unit);

            Assert.That(isActive, Is.True);
        }

        #endregion IsActive
    }
}
