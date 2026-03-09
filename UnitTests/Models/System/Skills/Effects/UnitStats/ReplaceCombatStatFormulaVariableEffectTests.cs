using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;

namespace UnitTests.Models.System.Skills.Effects.UnitStats
{
    public class ReplaceCombatStatFormulaVariableEffectTests
    {
        #region Constructor

        [Test]
        public void Constructor_Null()
        {
            IEnumerable<string> parameters = new List<string>();

            Assert.Throws<SkillEffectMissingParameterException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_1EmptyString()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_2EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty };

            Assert.Throws<SkillEffectMissingParameterException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_3EmptyStrings()
        {
            IEnumerable<string> parameters = new List<string>() { string.Empty, string.Empty, string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_VariablesToReplace_Empty()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat", string.Empty, "{UnitStat[Spd]}" };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_VariablesToUse_Empty()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat", "{UnitStat[Str]}", string.Empty };

            Assert.Throws<RequiredValueNotProvidedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_VariablesToReplace_Mismatched()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat", "{UnitStat[Str]},{UnitStat[Mag]}", "{UnitStat[Spd]}" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_VariablesToUse_Mismatched()
        {
            IEnumerable<string> parameters = new List<string>() { "Stat", "{UnitStat[Str]}", "{UnitStat[Spd]},{UnitStat[Def]}" };

            Assert.Throws<ParameterLengthsMismatchedException>(() => new ReplaceCombatStatFormulaVariableEffect(parameters));
        }

        [Test]
        public void Constructor_UnitCombatStat()
        {
            string stat = "Stat1";
            string variablesToReplace = "{UnitStat[Str]}";
            string variablesToUse = "{UnitCombatStat[Atk]}";

            IEnumerable<string> parameters = new List<string>() { stat, variablesToReplace, variablesToUse };

            ReplaceCombatStatFormulaVariableEffect effect = new ReplaceCombatStatFormulaVariableEffect(parameters);

            Assert.That(effect.Stats, Is.EqualTo(new List<string>() { stat }));
            Assert.That(effect.VariablesToReplace, Is.EqualTo(new List<string>() { variablesToReplace }));
            Assert.That(effect.VariablesToUse, Is.EqualTo(new List<string>() { variablesToUse }));

            Assert.That(effect.ParserOptions.EvalUnitCombatStat, Is.True);
            Assert.That(effect.ParserOptions.EvalUnitStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitLevel, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponStat, Is.False);
            Assert.That(effect.ParserOptions.EvalBattalionStat, Is.False);
            Assert.That(effect.ExecutionOrder, Is.EqualTo(SkillEffectExecutionOrder.Standard));
        }

        [Test]
        public void Constructor_UnitStat()
        {
            string stat = "Stat1";
            string variablesToReplace = "{UnitStat[Mag]}";
            string variablesToUse = "{UnitStat[Str]}";

            IEnumerable<string> parameters = new List<string>() { stat, variablesToReplace, variablesToUse };

            IReplaceCombatStatFormulaVariableEffect effect = new ReplaceCombatStatFormulaVariableEffect(parameters);

            Assert.That(effect.Stats, Is.EqualTo(new List<string>() { stat }));
            Assert.That(effect.VariablesToReplace, Is.EqualTo(new List<string>() { variablesToReplace }));
            Assert.That(effect.VariablesToUse, Is.EqualTo(new List<string>() { variablesToUse }));

            Assert.That(effect.ParserOptions.EvalUnitCombatStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitStat, Is.True);
            Assert.That(effect.ParserOptions.EvalUnitLevel, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponStat, Is.False);
            Assert.That(effect.ParserOptions.EvalBattalionStat, Is.False);
        }

        [Test]
        public void Constructor_UnitLevel()
        {
            string stat = "Stat1";
            string variablesToReplace = "{UnitStat[Str]}";
            string variablesToUse = "{UnitLevel}";

            IEnumerable<string> parameters = new List<string>() { stat, variablesToReplace, variablesToUse };

            IReplaceCombatStatFormulaVariableEffect effect = new ReplaceCombatStatFormulaVariableEffect(parameters);

            Assert.That(effect.Stats, Is.EqualTo(new List<string>() { stat }));
            Assert.That(effect.VariablesToReplace, Is.EqualTo(new List<string>() { variablesToReplace }));
            Assert.That(effect.VariablesToUse, Is.EqualTo(new List<string>() { variablesToUse }));

            Assert.That(effect.ParserOptions.EvalUnitCombatStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitLevel, Is.True);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponStat, Is.False);
            Assert.That(effect.ParserOptions.EvalBattalionStat, Is.False);
        }

        [Test]
        public void Constructor_WeaponUtilStat_Greatest()
        {
            string stat = "Stat1";
            string variablesToReplace = "{UnitStat[Str]}";
            string variablesToUse = "{WeaponUtilStat_Greatest}";

            IEnumerable<string> parameters = new List<string>() { stat, variablesToReplace, variablesToUse };

            IReplaceCombatStatFormulaVariableEffect effect = new ReplaceCombatStatFormulaVariableEffect(parameters);

            Assert.That(effect.Stats, Is.EqualTo(new List<string>() { stat }));
            Assert.That(effect.VariablesToReplace, Is.EqualTo(new List<string>() { variablesToReplace }));
            Assert.That(effect.VariablesToUse, Is.EqualTo(new List<string>() { variablesToUse }));

            Assert.That(effect.ParserOptions.EvalUnitCombatStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitLevel, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Greatest, Is.True);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponStat, Is.False);
            Assert.That(effect.ParserOptions.EvalBattalionStat, Is.False);
        }

        [Test]
        public void Constructor_WeaponUtilStat_Sum()
        {
            string stat = "Stat1";
            string variablesToReplace = "{UnitStat[Str]}";
            string variablesToUse = "{WeaponUtilStat_Sum}";

            IEnumerable<string> parameters = new List<string>() { stat, variablesToReplace, variablesToUse };

            IReplaceCombatStatFormulaVariableEffect effect = new ReplaceCombatStatFormulaVariableEffect(parameters);

            Assert.That(effect.Stats, Is.EqualTo(new List<string>() { stat }));
            Assert.That(effect.VariablesToReplace, Is.EqualTo(new List<string>() { variablesToReplace }));
            Assert.That(effect.VariablesToUse, Is.EqualTo(new List<string>() { variablesToUse }));

            Assert.That(effect.ParserOptions.EvalUnitCombatStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitLevel, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Sum, Is.True);
            Assert.That(effect.ParserOptions.EvalWeaponStat, Is.False);
            Assert.That(effect.ParserOptions.EvalBattalionStat, Is.False);
        }

        [Test]
        public void Constructor_WeaponStat()
        {
            string stat = "Stat1";
            string variablesToReplace = "{UnitStat[Str]}";
            string variablesToUse = "{WeaponStat[Mt]}";

            IEnumerable<string> parameters = new List<string>() { stat, variablesToReplace, variablesToUse };

            IReplaceCombatStatFormulaVariableEffect effect = new ReplaceCombatStatFormulaVariableEffect(parameters);

            Assert.That(effect.Stats, Is.EqualTo(new List<string>() { stat }));
            Assert.That(effect.VariablesToReplace, Is.EqualTo(new List<string>() { variablesToReplace }));
            Assert.That(effect.VariablesToUse, Is.EqualTo(new List<string>() { variablesToUse }));

            Assert.That(effect.ParserOptions.EvalUnitCombatStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitLevel, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponStat, Is.True);
            Assert.That(effect.ParserOptions.EvalBattalionStat, Is.False);
        }

        [Test]
        public void Constructor_BattalionStat()
        {
            string stat = "Stat1";
            string variablesToReplace = "{UnitStat[Str]}";
            string variablesToUse = "{BattalionStat[Mt]}";

            IEnumerable<string> parameters = new List<string>() { stat, variablesToReplace, variablesToUse };

            IReplaceCombatStatFormulaVariableEffect effect = new ReplaceCombatStatFormulaVariableEffect(parameters);

            Assert.That(effect.Stats, Is.EqualTo(new List<string>() { stat }));
            Assert.That(effect.VariablesToReplace, Is.EqualTo(new List<string>() { variablesToReplace }));
            Assert.That(effect.VariablesToUse, Is.EqualTo(new List<string>() { variablesToUse }));

            Assert.That(effect.ParserOptions.EvalUnitCombatStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitLevel, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Greatest, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponStat, Is.False);
            Assert.That(effect.ParserOptions.EvalBattalionStat, Is.True);
        }

        [Test]
        public void Constructor_MultipleVariablesToUse()
        {
            string stat = "Stat1";
            string variablesToReplace = "{UnitStat[Str]},{UnitStat[Str]},{UnitStat[Str]},{UnitStat[Str]}";
            string variablesToUse = "{UnitCombatStat[Atk]},{UnitLevel},{WeaponStat[Mt,Wt]},{EvalWeaponUtilStat_Greatest},";

            IEnumerable<string> parameters = new List<string>() { stat, variablesToReplace, variablesToUse };

            IReplaceCombatStatFormulaVariableEffect effect = new ReplaceCombatStatFormulaVariableEffect(parameters);

            Assert.That(effect.Stats, Is.EqualTo(new List<string>() { stat }));
            Assert.That(effect.VariablesToReplace, Is.EqualTo(variablesToReplace.Split(",")));
            Assert.That(effect.VariablesToUse, Is.EqualTo(new List<string>() { "{UnitCombatStat[Atk]}", "{UnitLevel}", "{WeaponStat[Mt,Wt]}", "{EvalWeaponUtilStat_Greatest}" }));

            Assert.That(effect.ParserOptions.EvalUnitCombatStat, Is.True);
            Assert.That(effect.ParserOptions.EvalUnitStat, Is.False);
            Assert.That(effect.ParserOptions.EvalUnitLevel, Is.True);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Greatest, Is.True);
            Assert.That(effect.ParserOptions.EvalWeaponUtilStat_Sum, Is.False);
            Assert.That(effect.ParserOptions.EvalWeaponStat, Is.True);
            Assert.That(effect.ParserOptions.EvalBattalionStat, Is.False);
        }

        #endregion Constructor
    }
}
