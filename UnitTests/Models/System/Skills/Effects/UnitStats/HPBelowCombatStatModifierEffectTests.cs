using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    public class HPBelowCombatStatModifierEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_3EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.Throws<PositiveIntegerException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_HPPercentage_Neg1()
        {
            List<string> parameters = new List<string>() { "-1", string.Empty, string.Empty };

            Assert.Throws<PositiveIntegerException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyStats()
        {
            List<string> parameters = new List<string>() { "1", string.Empty, "1" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_EmptyValues()
        {
            List<string> parameters = new List<string>() { "1", "Stat", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedStats()
        {
            List<string> parameters = new List<string>() { "1", "Stat 1,Stat 2", "1" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [Test]
        public void Constructor_MismatchedValues()
        {
            List<string> parameters = new List<string>() { "1", "Stat ", "1,2" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
