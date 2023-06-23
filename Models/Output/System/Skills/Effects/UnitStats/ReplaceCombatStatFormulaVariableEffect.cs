using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats
{
    public class ReplaceCombatStatFormulaVariableEffect : SkillEffect
    {

        #region Attributes

        protected override string Name { get { return "ReplaceCombatStatFormulaVariable"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The unit combat stats to be affected.
        /// </summary>
        public List<string> Stats { get; private set; }

        /// <summary>
        /// Param2. The variables to search for and replace in the formula.
        /// </summary>
        public List<string> VariablesToReplace { get; private set; }

        /// <summary>
        /// Param3. The variables to replace <c>VariablesToReplace</c> with.
        /// </summary>
        public List<string> VariablesToUse { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        /// <exception cref="ParameterLengthsMismatchedException"></exception>
        public ReplaceCombatStatFormulaVariableEffect(List<string> parameters)
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

            if (this.VariablesToReplace.Count != this.VariablesToUse.Count)
                throw new ParameterLengthsMismatchedException(NAME_PARAM_2, NAME_PARAM_3);
        }

        /// <summary>
        /// Due to the potential for fields to contain embedded CSVs (ex. "{UnitStat[Str,Mag]},{UnitStat[Spd]}"), we need a custom parser.
        /// </summary>
        private List<string> CustomCSVParse(List<string> data, int index)
        {
            List<string> output = new List<string>();
            string csv = (data.ElementAtOrDefault<string>(index) ?? string.Empty);

            //Eliminate all whitespace from the string, including interior whitespace
            csv = csv.Trim();
            csv = csv.Replace(" ", string.Empty);

            if (string.IsNullOrEmpty(csv))
                return output;

            foreach (string value in csv.Split("},{"))
            {
                if (!string.IsNullOrEmpty(value))
                    output.Add(value.Trim());
            }

            return output;
        }
    }
}
