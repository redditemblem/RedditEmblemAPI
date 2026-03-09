using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    public class StatModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new StatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new StatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new StatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyStats()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new StatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyValues()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new StatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedStats()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat 1,Stat 2", "1" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new StatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedValues()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat ", "1,2" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new StatModifierEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat1,Stat2", "1,2" };

            StatModifierEffect effect = new StatModifierEffect(parameters);

            Assert.That(effect.Modifiers.Count(), Is.EqualTo(2));
            Assert.That(effect.Modifiers.ContainsKey("Stat1"), Is.True);
            Assert.That(effect.Modifiers["Stat1"], Is.EqualTo(1));
            Assert.That(effect.Modifiers.ContainsKey("Stat2"), Is.True);
            Assert.That(effect.Modifiers["Stat2"], Is.EqualTo(2));
        }

        #endregion Constructor

        #region Apply

        [Test]
        public void Apply()
        {
            IUnit unit = Substitute.For<IUnit>();
            IUnitStatus status = Substitute.For<IUnitStatus>();
            IDictionary<string, ITag> tags = new Dictionary<string, ITag>();

            IEnumerable<string> parameters = new List<string>() { "Stat1", "1" };
            StatModifierEffect effect = new StatModifierEffect(parameters);

            effect.Apply(unit, status, tags);

            unit.Stats.Received(1).ApplyGeneralStatModifiers(Arg.Is<IDictionary<string, int>>(m => m["Stat1"] == 1), status.Status.Name, true);
        }

        #endregion Apply
    }
}