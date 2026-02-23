using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.Skills.Effects.MovementRange
{
    public class HPAboveIgnoreUnitAffiliationsEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_EmptyInput()
        {
            IEnumerable<string> data = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new HPAboveIgnoreUnitAffiliationsEffect(data));
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<string> data = new List<string>()
            { 
                "20"
            };

            IHPAboveIgnoreUnitAffiliationsEffect effect = new HPAboveIgnoreUnitAffiliationsEffect(data);

            Assert.That(effect.HPPercentage, Is.EqualTo(20));
        }

        #endregion Constructor

        #region IsActive

        [TestCase(50, 50, true)]
        [TestCase(50, 50.1, true)]
        [TestCase(50, 49.9, false)]
        public void IsActive(int effectHpPercentage, decimal unitHpPercentage, bool expected)
        {
            IUnit unit = Substitute.For<IUnit>();
            unit.Stats.HP.Percentage.Returns(unitHpPercentage);

            IEnumerable<string> data = new List<string>()
            {
                $"{effectHpPercentage}"
            };

            IIgnoreUnitAffiliations effect = new HPAboveIgnoreUnitAffiliationsEffect(data);
            bool isActive = effect.IsActive(unit);

            Assert.That(isActive, Is.EqualTo(expected));
        }

        #endregion IsActive
    }
}
