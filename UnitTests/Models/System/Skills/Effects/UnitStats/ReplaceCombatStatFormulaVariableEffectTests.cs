using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    [TestClass]
    public class ReplaceCombatStatFormulaVariableEffectTests
    {
        #region Constructor

        [TestMethod]
        public void ReplaceCombatStatFormulaVariableEffect_Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [TestMethod]
        public void ReplaceCombatStatFormulaVariableEffect_Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [TestMethod]
        public void ReplaceCombatStatFormulaVariableEffect_Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.ThrowsException<SkillEffectMissingParameterException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [TestMethod]
        public void ReplaceCombatStatFormulaVariableEffect_Constructor_3EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [TestMethod]
        public void ReplaceCombatStatFormulaVariableEffect_Constructor_VariablesToReplace_Empty()
        {
            List<string> parameters = new List<string>() { "Stat", string.Empty, "{UnitStat[Spd]}" };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [TestMethod]
        public void ReplaceCombatStatFormulaVariableEffect_Constructor_VariablesToUse_Empty()
        {
            List<string> parameters = new List<string>() { "Stat", "{UnitStat[Str]}", string.Empty };

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [TestMethod]
        public void ReplaceCombatStatFormulaVariableEffect_Constructor_VariablesToReplace_Mismatched()
        {
            List<string> parameters = new List<string>() { "Stat", "{UnitStat[Str]},{UnitStat[Mag]}", "{UnitStat[Spd]}" };

            Assert.ThrowsException<ParameterLengthsMismatchedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [TestMethod]
        public void ReplaceCombatStatFormulaVariableEffect_Constructor_VariablesToUse_Mismatched()
        {
            List<string> parameters = new List<string>() { "Stat", "{UnitStat[Str]}", "{UnitStat[Spd]},{UnitStat[Def]}" };

            Assert.ThrowsException<ParameterLengthsMismatchedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        #endregion Constructor
    }
}
