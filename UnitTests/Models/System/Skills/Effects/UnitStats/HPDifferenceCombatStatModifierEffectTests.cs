using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    public class HPDifferenceCombatStatModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new HPDifferenceCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new HPDifferenceCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty, };

            Assert.Throws<NonZeroPositiveDecimalException>(() => new HPDifferenceCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_Multiplier_Neg1()
        {
            IEnumerable<string> parameters = new List<string>() { "-1", string.Empty };

            Assert.Throws<NonZeroPositiveDecimalException>(() => new HPDifferenceCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_Multiplier_0()
        {
            IEnumerable<string> parameters = new List<string>() { "0", string.Empty };

            Assert.Throws<NonZeroPositiveDecimalException>(() => new HPDifferenceCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyStats()
        {
            IEnumerable<string> parameters = new List<string>() { "1", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new HPDifferenceCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<string> parameters = new List<string>() { "1.5", "Stat1,Stat2" };

            HPDifferenceCombatStatModifierEffect effect = new HPDifferenceCombatStatModifierEffect(parameters);

            Assert.That(effect.Multiplier, Is.EqualTo(1.5m));
            Assert.That(effect.Stats, Is.EqualTo(new List<string>() { "Stat1", "Stat2" }));
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.Standard));
        }

        #endregion Constructor

        #region Apply

        [Test]
        public void Apply_NoHpDifference()
        {
            IEnumerable<string> parameters = new List<string>() { "1.5", "Stat1" };

            HPDifferenceCombatStatModifierEffect effect = new HPDifferenceCombatStatModifierEffect(parameters);

            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            unit.Stats.HP.Difference.Returns(0);

            effect.Apply(unit, skill, map, units);

            unit.Stats.DidNotReceiveWithAnyArgs().ApplyCombatStatModifiers(Arg.Any<IDictionary<string, int>>(), skill.Name, true);
        }

        [Test]
        public void Apply()
        {
            IEnumerable<string> parameters = new List<string>() { "1.75", "Stat1" };

            HPDifferenceCombatStatModifierEffect effect = new HPDifferenceCombatStatModifierEffect(parameters);

            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            unit.Stats.HP.Difference.Returns(2);

            effect.Apply(unit, skill, map, units);

            unit.Stats.Received(1).ApplyCombatStatModifiers(Arg.Is<IDictionary<string, int>>(m => m["Stat1"] == 3), skill.Name, true);
        }

        #endregion Apply
    }
}
