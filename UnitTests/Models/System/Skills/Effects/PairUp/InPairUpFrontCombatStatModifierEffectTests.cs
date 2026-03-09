using NSubstitute;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.PairUp;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.System.Skills.Effects.PairUp
{
    public class InPairUpFrontCombatStatModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyStats()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyValues()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedStats()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat 1,Stat 2", "1" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedValues()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat ", "1,2" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat1,Stat2", "1,2" };

            InPairUpFrontCombatStatModifierEffect effect = new InPairUpFrontCombatStatModifierEffect(parameters);

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
        public void Apply_BackOfPairUp()
        {
            IUnit frontOfPair = Substitute.For<IUnit>();
            IUnit backOfPair = Substitute.For<IUnit>();

            frontOfPair.Location.PairedUnitObj.Returns(backOfPair);
            frontOfPair.Location.IsBackOfPair.Returns(false);
            backOfPair.Location.PairedUnitObj.Returns(frontOfPair);
            backOfPair.Location.IsBackOfPair.Returns(true);

            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit> { frontOfPair, backOfPair };

            IEnumerable<string> parameters = new List<string>() { "Stat1", "1" };
            InPairUpFrontCombatStatModifierEffect effect = new InPairUpFrontCombatStatModifierEffect(parameters);

            effect.Apply(backOfPair, skill, map, units);

            frontOfPair.Stats.DidNotReceiveWithAnyArgs().ApplyCombatStatModifiers(Arg.Any<IDictionary<string, int>>(), Arg.Any<string>(), true);
            backOfPair.Stats.DidNotReceiveWithAnyArgs().ApplyCombatStatModifiers(Arg.Any<IDictionary<string, int>>(), Arg.Any<string>(), true);
        }

        [Test]
        public void Apply_FrontOfPairUp()
        {
            IUnit frontOfPair = Substitute.For<IUnit>();
            IUnit backOfPair = Substitute.For<IUnit>();

            frontOfPair.Location.PairedUnitObj.Returns(backOfPair);
            frontOfPair.Location.IsBackOfPair.Returns(false);
            backOfPair.Location.PairedUnitObj.Returns(frontOfPair);
            backOfPair.Location.IsBackOfPair.Returns(true);

            ISkill skill = Substitute.For<ISkill>();
            IMapObj map = Substitute.For<IMapObj>();
            List<IUnit> units = new List<IUnit> { frontOfPair, backOfPair };

            IEnumerable<string> parameters = new List<string>() { "Stat1", "1" };
            InPairUpFrontCombatStatModifierEffect effect = new InPairUpFrontCombatStatModifierEffect(parameters);

            effect.Apply(frontOfPair, skill, map, units);

            frontOfPair.Stats.Received(1).ApplyCombatStatModifiers(Arg.Is<IDictionary<string, int>>(m => m["Stat1"] == 1), Arg.Any<string>(), true);
            backOfPair.Stats.DidNotReceiveWithAnyArgs().ApplyCombatStatModifiers(Arg.Any<IDictionary<string, int>>(), Arg.Any<string>(), true);
        }

        #endregion Apply
    }
}
