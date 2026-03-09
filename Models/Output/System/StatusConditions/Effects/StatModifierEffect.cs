using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    #region Interface

    /// <inheritdoc cref="StatModifierEffect"/>
    public interface IStatModifierEffect
    {
        /// <inheritdoc cref="StatModifierEffect.Modifiers"/>
        IDictionary<string, int> Modifiers { get; }
    }

    #endregion Interface

    public class StatModifierEffect : StatusConditionEffect, IStatModifierEffect
    {
        #region Attributes

        protected override string Name { get { return "StatModifier"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1/Param2. The unit stat modifiers to apply.
        /// </summary>
        public IDictionary<string, int> Modifiers { get; private set; }

        #endregion Attributes

        public StatModifierEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.Modifiers = DataParser.StatValueCSVs_Int_Any(parameters, INDEX_PARAM_1, NAME_PARAM_1, INDEX_PARAM_2, NAME_PARAM_2);
        }

        /// <summary>
        /// Applies <c>Modifiers</c> to <paramref name="unit"/>.
        /// </summary>
        public override void Apply(IUnit unit, IUnitStatus status, IDictionary<string, ITag> tags)
        {
            unit.Stats.ApplyGeneralStatModifiers(this.Modifiers, status.Status.Name, true);
        }
    }
}
