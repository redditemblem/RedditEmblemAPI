using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    [TestClass]
    public class HPBelowCombatStatModifierEffectTests
    {
        #region Constructor

        [TestMethod]
        public void HPBelowCombatStatModifierEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowCombatStatModifierEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowCombatStatModifierEffect_Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowCombatStatModifierEffect_Constructor_3EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.ThrowsException<PositiveIntegerException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowCombatStatModifierEffect_Constructor_HPPercentage_Neg1()
        {
            List<string> parameters = new List<string>() { "-1", string.Empty, string.Empty };

            Assert.ThrowsException<PositiveIntegerException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowCombatStatModifierEffect_Constructor_EmptyStats()
        {
            List<string> parameters = new List<string>() { "1", string.Empty, "1" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowCombatStatModifierEffect_Constructor_EmptyValues()
        {
            List<string> parameters = new List<string>() { "1", "Stat", string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowCombatStatModifierEffect_Constructor_MismatchedStats()
        {
            List<string> parameters = new List<string>() { "1", "Stat 1,Stat 2", "1" };

            Assert.ThrowsException<ParameterLengthsMismatchedException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowCombatStatModifierEffect_Constructor_MismatchedValues()
        {
            List<string> parameters = new List<string>() { "1", "Stat ", "1,2" };

            Assert.ThrowsException<ParameterLengthsMismatchedException>(() => new HPBelowCombatStatModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
