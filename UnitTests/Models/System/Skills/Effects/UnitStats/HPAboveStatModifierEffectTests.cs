using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    public class HPAboveStatModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new HPAboveStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new HPAboveStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new HPAboveStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_3EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.Throws<PositiveIntegerException>(() => new HPAboveStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_HPPercentage_Neg1()
        {
            IEnumerable<string> parameters = new List<string>() { "-1", string.Empty, string.Empty };

            Assert.Throws<PositiveIntegerException>(() => new HPAboveStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyStats()
        {
            IEnumerable<string> parameters = new List<string>() { "1", string.Empty, "1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new HPAboveStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyValues()
        {
            IEnumerable<string> parameters = new List<string>() { "1", "Stat", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new HPAboveStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedStats()
        {
            IEnumerable<string> parameters = new List<string>() { "1", "Stat 1,Stat 2", "1" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new HPAboveStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedValues()
        {
            IEnumerable<string> parameters = new List<string>() { "1", "Stat ", "1,2" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new HPAboveStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<string> parameters = new List<string>() { "50", "Stat1,Stat2", "1,2" };

            HPAboveStatModifierEffect effect = new HPAboveStatModifierEffect(parameters);

            Assert.That(effect.HPPercentage, Is.EqualTo(50));
            Assert.That(effect.Modifiers.Count(), Is.EqualTo(2));
            Assert.That(effect.Modifiers.ContainsKey("Stat1"), Is.True);
            Assert.That(effect.Modifiers["Stat1"], Is.EqualTo(1));
            Assert.That(effect.Modifiers.ContainsKey("Stat2"), Is.True);
            Assert.That(effect.Modifiers["Stat2"], Is.EqualTo(2));
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.Standard));
        }

        #endregion Constructor

        #region Apply

        [TestCase(49.9)]
        public void Apply_ModifiersDidNotApply(decimal unitHpPercentage)
        {
            IEnumerable<string> parameters = new List<string>() { "50", "Stat1 ", "1" };

            HPAboveStatModifierEffect effect = new HPAboveStatModifierEffect(parameters);

            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            unit.Stats.HP.Percentage.Returns(unitHpPercentage);

            effect.Apply(unit, skill, map, units);

            unit.Stats.DidNotReceiveWithAnyArgs().ApplyGeneralStatModifiers(effect.Modifiers, skill.Name, true);
        }

        [TestCase(50)]
        [TestCase(50.1)]
        public void Apply_ModifiersApplied(decimal unitHpPercentage)
        {
            IEnumerable<string> parameters = new List<string>() { "50", "Stat1 ", "1" };

            HPAboveStatModifierEffect effect = new HPAboveStatModifierEffect(parameters);

            IUnit unit = Substitute.For<IUnit>();
            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit>() { unit };

            unit.Stats.HP.Percentage.Returns(unitHpPercentage);

            effect.Apply(unit, skill, map, units);

            unit.Stats.Received(1).ApplyGeneralStatModifiers(effect.Modifiers, skill.Name, true);
        }

        #endregion Apply
    }
}
