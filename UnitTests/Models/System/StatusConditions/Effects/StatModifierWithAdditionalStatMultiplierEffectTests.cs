using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.StatusConditions.Effects
{
    public class StatModifierWithAdditionalStatMultiplierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new StatModifierWithAdditionalStatMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new StatModifierWithAdditionalStatMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<StatusConditionEffectMissingParameterException>(() => new StatModifierWithAdditionalStatMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_3EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new StatModifierWithAdditionalStatMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyStats()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "1", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new StatModifierWithAdditionalStatMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyValues()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat", string.Empty, string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new StatModifierWithAdditionalStatMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedValues()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat ", "1,2", string.Empty };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new StatModifierWithAdditionalStatMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyAdditionalStatName()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat1,Stat2", "1,2", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new StatModifierWithAdditionalStatMultiplierEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat1,Stat2", "1,2", "Intensity" };

            StatModifierWithAdditionalStatMultiplierEffect effect = new StatModifierWithAdditionalStatMultiplierEffect(parameters);

            Assert.That(effect.Modifiers.Count(), Is.EqualTo(2));
            Assert.That(effect.Modifiers.ContainsKey("Stat1"), Is.True);
            Assert.That(effect.Modifiers["Stat1"], Is.EqualTo(1));
            Assert.That(effect.Modifiers.ContainsKey("Stat2"), Is.True);
            Assert.That(effect.Modifiers["Stat2"], Is.EqualTo(2));
            Assert.That(effect.AdditionalStatName, Is.EqualTo("Intensity"));
        }

        #endregion Constructor

        #region Apply

        [Test]
        public void Apply_AdditionalStatDoesNotExist()
        {
            IUnit unit = Substitute.For<IUnit>();
            IUnitStatus status = Substitute.For<IUnitStatus>();
            IDictionary<string, ITag> tags = new Dictionary<string, ITag>();

            status.AdditionalStats.Returns(new Dictionary<string, int>());

            IEnumerable<string> parameters = new List<string>() { "Stat1", "1", "Intensity" };
            StatModifierWithAdditionalStatMultiplierEffect effect = new StatModifierWithAdditionalStatMultiplierEffect(parameters);

            effect.Apply(unit, status, tags);

            unit.Stats.DidNotReceiveWithAnyArgs().ApplyGeneralStatModifiers(Arg.Any<IDictionary<string, int>>(), Arg.Any<string>(), true);
        }

        [Test]
        public void Apply()
        {
            IUnit unit = Substitute.For<IUnit>();
            IUnitStatus status = Substitute.For<IUnitStatus>();
            IDictionary<string, ITag> tags = new Dictionary<string, ITag>();

            IDictionary<string, int> additionalStats = new Dictionary<string, int>();
            additionalStats.Add("Intensity", 2);
            status.AdditionalStats.Returns(additionalStats);

            IEnumerable<string> parameters = new List<string>() { "Stat1", "3", "Intensity" };
            StatModifierWithAdditionalStatMultiplierEffect effect = new StatModifierWithAdditionalStatMultiplierEffect(parameters);

            effect.Apply(unit, status, tags);

            unit.Stats.Received(1).ApplyGeneralStatModifiers(Arg.Is<IDictionary<string, int>>(m => m["Stat1"] == 6), status.Status.Name, true);
        }

        #endregion Apply
    }
}
