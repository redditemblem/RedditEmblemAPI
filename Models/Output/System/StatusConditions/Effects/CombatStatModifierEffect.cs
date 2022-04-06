using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    public class CombatStatModifierEffect : StatusConditionEffect
    {
        #region Attributes

        protected override string Name { get { return "CombatStatModifier"; } }
        protected override int ParameterCount { get { return 2; } }

        private IList<string> Stats { get; set; }
        private IList<int> Values { get; set; }

        #endregion

        public CombatStatModifierEffect(IList<string> parameters)
            : base(parameters)
        {
            this.Stats = DataParser.List_StringCSV(parameters, 0); //Param1
            this.Values = DataParser.List_IntCSV(parameters, 1, "Param2", false);

            if (this.Stats.Count == 0)
                throw new RequiredValueNotProvidedException("Param1");
            if (this.Values.Count == 0)
                throw new RequiredValueNotProvidedException("Param2");

            if (this.Stats.Count != this.Values.Count)
                throw new SkillEffectParameterLengthsMismatchedException("Param1", "Param2");
        }

        /// <summary>
        /// Adds the items in <c>Values</c> as modifiers to the combat stats in <c>Stats</c> for <paramref name="unit"/>.
        /// </summary>
        public override void Apply(Unit unit, StatusCondition status)
        {
            ApplyUnitCombatStatModifiers(unit, status.Name, this.Stats, this.Values);
        }
    }
}
