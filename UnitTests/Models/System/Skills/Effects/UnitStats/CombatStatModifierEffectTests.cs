using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    [TestClass]
    public class CombatStatModifierEffectTests
    {
        #region Constructor

        [TestMethod]
        public void CombatStatModifierEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new CombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void CombatStatModifierEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new CombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void CombatStatModifierEffect_Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new CombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void CombatStatModifierEffect_Constructor_EmptyStats()
        {
            List<string> parameters = new List<string>() { string.Empty, "1" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new CombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void CombatStatModifierEffect_Constructor_EmptyValues()
        {
            List<string> parameters = new List<string>() { "Stat", string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new CombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void CombatStatModifierEffect_Constructor_MismatchedStats()
        {
            List<string> parameters = new List<string>() { "Stat 1,Stat 2", "1" };

            Assert.ThrowsException<ParameterLengthsMismatchedException>(() => new CombatStatModifierEffect(parameters));
        }

        [TestMethod]
        public void CombatStatModifierEffect_Constructor_MismatchedValues()
        {
            List<string> parameters = new List<string>() { "Stat ", "1,2" };

            Assert.ThrowsException<ParameterLengthsMismatchedException>(() => new CombatStatModifierEffect(parameters));
        }

        #endregion Constructor
    }
}
