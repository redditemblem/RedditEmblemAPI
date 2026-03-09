using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    public class CombatStatModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyStats()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyValues()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedStats()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat 1,Stat 2", "1" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedValues()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat ", "1,2" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat1,Stat2", "1,2" };

            CombatStatModifierEffect effect = new CombatStatModifierEffect(parameters);

            Assert.That(effect.Modifiers.Count(), Is.EqualTo(2));
            Assert.That(effect.Modifiers.ContainsKey("Stat1"), Is.True);
            Assert.That(effect.Modifiers["Stat1"], Is.EqualTo(1));
            Assert.That(effect.Modifiers.ContainsKey("Stat2"), Is.True);
            Assert.That(effect.Modifiers["Stat2"], Is.EqualTo(2));
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.Standard));
        }

        #endregion Constructor

        #region Apply

        [Test]
        public void Apply()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat1", "1" };

            CombatStatModifierEffect effect = new CombatStatModifierEffect(parameters);

            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            effect.Apply(unit, skill, map, units);

            unit.Stats.Received(1).ApplyCombatStatModifiers(effect.Modifiers, skill.Name, true);
        }

        #endregion Apply
    }
}
