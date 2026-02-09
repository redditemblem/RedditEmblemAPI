using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.PairUp;

namespace UnitTests.Models.System.Skills.Effects.PairUp
{
    public class InPairUpFrontCombatStatModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyStats()
        {
            List<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyValues()
        {
            List<string> parameters = new List<string>() { "Stat", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedStats()
        {
            List<string> parameters = new List<string>() { "Stat 1,Stat 2", "1" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedValues()
        {
            List<string> parameters = new List<string>() { "Stat ", "1,2" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new InPairUpFrontCombatStatModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
