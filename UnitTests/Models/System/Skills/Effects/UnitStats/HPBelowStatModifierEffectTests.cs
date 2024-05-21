using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    [TestClass]
    public class HPBelowStatModifierEffectTests
    {
        #region Constructor

        [TestMethod]
        public void HPBelowStatModifierEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new HPBelowStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowStatModifierEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new HPBelowStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowStatModifierEffect_Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new HPBelowStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowStatModifierEffect_Constructor_3EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.ThrowsException<PositiveIntegerException>(() => new HPBelowStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowStatModifierEffect_Constructor_HPPercentage_Neg1()
        {
            List<string> parameters = new List<string>() { "-1", string.Empty, string.Empty };

            Assert.ThrowsException<PositiveIntegerException>(() => new HPBelowStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowStatModifierEffect_Constructor_EmptyStats()
        {
            List<string> parameters = new List<string>() { "1", string.Empty, "1" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new HPBelowStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowStatModifierEffect_Constructor_EmptyValues()
        {
            List<string> parameters = new List<string>() { "1", "Stat", string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new HPBelowStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowStatModifierEffect_Constructor_MismatchedStats()
        {
            List<string> parameters = new List<string>() { "1", "Stat 1,Stat 2", "1" };

            Assert.ThrowsException<ParameterLengthsMismatchedException>(() => new HPBelowStatModifierEffect(parameters));
        }

        [TestMethod]
        public void HPBelowStatModifierEffect_Constructor_MismatchedValues()
        {
            List<string> parameters = new List<string>() { "1", "Stat ", "1,2" };

            Assert.ThrowsException<ParameterLengthsMismatchedException>(() => new HPBelowStatModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
