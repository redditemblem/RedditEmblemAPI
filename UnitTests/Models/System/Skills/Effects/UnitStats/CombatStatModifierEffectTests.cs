using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    public class CombatStatModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyStats()
        {
            List<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyValues()
        {
            List<string> parameters = new List<string>() { "Stat", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedStats()
        {
            List<string> parameters = new List<string>() { "Stat 1,Stat 2", "1" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new CombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedValues()
        {
            List<string> parameters = new List<string>() { "Stat ", "1,2" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new CombatStatModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
