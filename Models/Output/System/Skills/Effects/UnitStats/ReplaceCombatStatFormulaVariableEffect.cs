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
        public IList<string> Stats { get; private set; }

        /// <summary>
        /// Param2. The variables to search for and replace in the formula.
        /// </summary>
        public IList<string> VariablesToReplace { get; private set; }

        /// <summary>
        /// Param3. The variables to replace <c>VariablesToReplace</c> with.
        /// </summary>
        public IList<string> VariablesToUse { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        /// <exception cref="SkillEffectParameterLengthsMismatchedException"></exception>
        public ReplaceCombatStatFormulaVariableEffect(IList<string> parameters)
            : base(parameters)
        {
            this.Stats = ParseHelper.StringCSVParse(parameters, 0); //Param1
            this.VariablesToReplace = CustomCSVParse(parameters, 1); //Param2
            this.VariablesToUse = CustomCSVParse(parameters, 2); //Param3


            if (this.Stats.Count == 0)
                throw new RequiredValueNotProvidedException("Param1");
            if (this.VariablesToReplace.Count == 0)
                throw new RequiredValueNotProvidedException("Param2");
            if (this.VariablesToUse.Count == 0)
                throw new RequiredValueNotProvidedException("Param3");

            if (this.VariablesToReplace.Count != this.VariablesToUse.Count)
                throw new SkillEffectParameterLengthsMismatchedException("Param2", "Param3");
        }

        /// <summary>
        /// Due to the potential for fields to contain embedded CSVs (ex. "{UnitStat[Str,Mag]},{UnitStat[Spd]}"), we need a custom parser.
        /// </summary>
        private IList<string> CustomCSVParse(IList<string> data, int index)
        {
            IList<string> output = new List<string>();
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
