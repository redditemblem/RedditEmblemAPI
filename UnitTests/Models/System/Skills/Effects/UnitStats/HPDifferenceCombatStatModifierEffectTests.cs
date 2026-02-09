using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    public class HPDifferenceCombatStatModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new HPDifferenceCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new HPDifferenceCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, };

            Assert.Throws<NonZeroPositiveDecimalException>(() => new HPDifferenceCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_Multiplier_Neg1()
        {
            List<string> parameters = new List<string>() { "-1", string.Empty };

            Assert.Throws<NonZeroPositiveDecimalException>(() => new HPDifferenceCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_Multiplier_0()
        {
            List<string> parameters = new List<string>() { "0", string.Empty };

            Assert.Throws<NonZeroPositiveDecimalException>(() => new HPDifferenceCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyStats()
        {
            List<string> parameters = new List<string>() { "1", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new HPDifferenceCombatStatModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
