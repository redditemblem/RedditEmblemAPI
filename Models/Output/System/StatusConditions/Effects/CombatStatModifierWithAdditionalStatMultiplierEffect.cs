using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    public class CombatStatModifierWithAdditionalStatMultiplierEffect : StatusConditionEffect
    {
        #region Attributes

        protected override string Name { get { return "CombatStatModifierWithAdditionalStatMultiplier"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1/Param2. The unit combat stat modifiers to apply.
        /// </summary>
        private IDictionary<string, int> Modifiers { get; set; }

        /// <summary>
        /// Param3. The name of the status condition additional stat.
        /// </summary>
        private string AdditionalStatName { get; set; }

        #endregion

        public CombatStatModifierWithAdditionalStatMultiplierEffect(List<string> parameters)
            : base(parameters)
        {
            this.Modifiers = DataParser.StatValueCSVs_Int_Any(parameters, INDEX_PARAM_1, NAME_PARAM_1, INDEX_PARAM_2, NAME_PARAM_2);
            this.AdditionalStatName = DataParser.String(parameters, INDEX_PARAM_3, NAME_PARAM_3);
        }

        /// <summary>
        /// Applies <c>Modifiers</c> to <paramref name="unit"/>.
        /// </summary>
        public override void Apply(IUnit unit, IUnitStatus status, IDictionary<string, ITag> tags)
        {
            //Search for the additional stat on the status condition
            int multiplier;
            if (!status.AdditionalStats.TryGetValue(this.AdditionalStatName, out multiplier))
                return;

            //Make a copy of the modifier dictionary
            Dictionary<string, int> modifiers = this.Modifiers.ToDictionary();
            foreach(KeyValuePair<string, int> modifier in modifiers)
                modifiers[modifier.Key] = modifier.Value * multiplier;

            unit.Stats.ApplyCombatStatModifiers(modifiers, status.StatusObj.Name, true);
        }
    }
}
