using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats
{
    #region Interface

    /// <inheritdoc cref="HPBelowStatModifierEffect"/>
    public interface IHPBelowStatModifierEffect
    {
        /// <inheritdoc cref="HPBelowStatModifierEffect.HPPercentage"/>
        int HPPercentage { get; }

        /// <inheritdoc cref="HPBelowStatModifierEffect.Modifiers"/>
        IDictionary<string, int> Modifiers { get; }
    }

    #endregion Interface

    public class HPBelowStatModifierEffect : SkillEffect, IHPBelowStatModifierEffect
    {
        #region Attributes

        protected override string Name { get { return "HPBelowStatModifier"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The maximum HP percentage the unit can have.
        /// </summary>
        public int HPPercentage { get; private set; }

        /// <summary>
        /// Param2. The unit stat modifiers to apply.
        /// </summary>
        public IDictionary<string, int> Modifiers { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public HPBelowStatModifierEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.HPPercentage = DataParser.Int_Positive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
            this.Modifiers = DataParser.StatValueCSVs_Int_Any(parameters, INDEX_PARAM_2, NAME_PARAM_2, INDEX_PARAM_3, NAME_PARAM_3);
        }

        /// <summary>
        /// Applies <c>Modifiers</c> to <paramref name="unit"/> if <paramref name="unit"/>'s HP percentage is equal to or below the value of <c>HPPercentage</c>.
        /// </summary>
        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            //HP percentage must be equal to or below threshold
            if (unit.Stats.HP.Percentage > this.HPPercentage)
                return;

            unit.Stats.ApplyGeneralStatModifiers(this.Modifiers, skill.Name, true);
        }
    }
}
