using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    public class ReplaceCombatStatFormulaVariableEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            List<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            List<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_3EmptyStrings()
        {
            List<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_VariablesToReplace_Empty()
        {
            List<string> parameters = new List<string>() { "Stat", string.Empty, "{UnitStat[Spd]}" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_VariablesToUse_Empty()
        {
            List<string> parameters = new List<string>() { "Stat", "{UnitStat[Str]}", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_VariablesToReplace_Mismatched()
        {
            List<string> parameters = new List<string>() { "Stat", "{UnitStat[Str]},{UnitStat[Mag]}", "{UnitStat[Spd]}" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_VariablesToUse_Mismatched()
        {
            List<string> parameters = new List<string>() { "Stat", "{UnitStat[Str]}", "{UnitStat[Spd]},{UnitStat[Def]}" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        #endregion Constructor
    }
}
