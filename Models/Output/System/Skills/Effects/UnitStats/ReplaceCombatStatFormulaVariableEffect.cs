using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;
using System.Collections.Generic;
using System.Linq;
using Regex = System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats
{
    #region Interface

    /// <inheritdoc cref="ReplaceCombatStatFormulaVariableEffect"/>
    public interface IReplaceCombatStatFormulaVariableEffect
    {
        /// <inheritdoc cref="ReplaceCombatStatFormulaVariableEffect.Stats"/>
        IEnumerable<string> Stats { get; }

        /// <inheritdoc cref="ReplaceCombatStatFormulaVariableEffect.VariablesToReplace"/>
        IEnumerable<string> VariablesToReplace { get; }

        /// <inheritdoc cref="ReplaceCombatStatFormulaVariableEffect.VariablesToUse"/>
        IEnumerable<string> VariablesToUse { get; }

        /// <inheritdoc cref="ReplaceCombatStatFormulaVariableEffect.ParserOptions"/>
        EquationParserOptions ParserOptions { get; }
    }

    #endregion Interface

    public class ReplaceCombatStatFormulaVariableEffect : SkillEffect, IReplaceCombatStatFormulaVariableEffect
    {
        #region Constants

        private static Regex.Regex variableRegex = new Regex.Regex("{[a-zA-Z\\,_\\[\\]]+}");
        
        #endregion Constants

        #region Attributes

        protected override string Name { get { return "ReplaceCombatStatFormulaVariable"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The unit combat stats to be affected.
        /// </summary>
        public IEnumerable<string> Stats { get; private set; }

        /// <summary>
        /// Param2. The variables to search for and replace in the formula.
        /// </summary>
        public IEnumerable<string> VariablesToReplace { get; private set; }

        /// <summary>
        /// Param3. The variables to replace <c>VariablesToReplace</c> with.
        /// </summary>
        public IEnumerable<string> VariablesToUse { get; private set; }

        /// <summary>
        /// <c>EquationParserOptions</c> settings to apply to any equations impacted by this effect.
        /// </summary>
        public EquationParserOptions ParserOptions { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        /// <exception cref="ParameterLengthsMismatchedException"></exception>
        public ReplaceCombatStatFormulaVariableEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.Stats = DataParser.List_StringCSV(parameters, INDEX_PARAM_1);
            this.VariablesToReplace = CustomCSVParse(parameters, INDEX_PARAM_2);
            this.VariablesToUse = CustomCSVParse(parameters, INDEX_PARAM_3);

            if (!this.Stats.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_1);
            if (!this.VariablesToReplace.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_2);
            if (!this.VariablesToUse.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_3);

            if (this.VariablesToReplace.Count() != this.VariablesToUse.Count())
                throw new ParameterLengthsMismatchedException(NAME_PARAM_2, NAME_PARAM_3);

            this.ParserOptions = GetEquationParserOptions();
        }

        /// <summary>
        /// Determines which <c>EquationParserOptions</c> settings to flag based on <c>this.VariablesToUse</c> and returns an options object.
        /// </summary>
        private EquationParserOptions GetEquationParserOptions()
        {
            return new EquationParserOptions()
            {
                EvalUnitCombatStat = this.VariablesToUse.Any(v => v.Contains("UnitCombatStat")),
                EvalUnitStat = this.VariablesToUse.Any(v => v.Contains("UnitStat")),
                EvalUnitLevel = this.VariablesToUse.Any(v => v.Contains("UnitLevel")),
                EvalWeaponUtilStat_Greatest = this.VariablesToUse.Any(v => v.Contains("WeaponUtilStat_Greatest")),
                EvalWeaponUtilStat_Sum = this.VariablesToUse.Any(v => v.Contains("WeaponUtilStat_Sum")),
                EvalWeaponStat = this.VariablesToUse.Any(v => v.Contains("WeaponStat")),
                EvalBattalionStat = this.VariablesToUse.Any(v => v.Contains("BattalionStat"))
            };
        }

        /// <summary>
        /// Due to the potential for fields to contain embedded CSVs (ex. "{UnitStat[Str,Mag]},{UnitStat[Spd]}"), we need a custom parser.
        /// </summary>
        private IEnumerable<string> CustomCSVParse(IEnumerable<string> data, int index)
        {
            List<string> output = new List<string>();
            string csv = data.ElementAtOrDefault(index) ?? string.Empty;

            //Eliminate all whitespace from the string, including interior whitespace
            csv = csv.Trim();
            csv = csv.Replace(" ", string.Empty);

            if (string.IsNullOrEmpty(csv))
                return output;

            Regex.MatchCollection matches = variableRegex.Matches(csv);
            foreach (Regex.Match variable in matches)
                output.Add(variable.Value);

            return output;
        }
    }
}
