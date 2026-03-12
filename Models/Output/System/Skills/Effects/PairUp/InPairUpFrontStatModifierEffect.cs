using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.PairUp
{
    #region Interface

    /// <inheritdoc cref="InPairUpFrontStatModifierEffect"/>
    public interface IInPairUpFrontStatModifierEffect
    {
        /// <inheritdoc cref="InPairUpFrontStatModifierEffect.Modifiers"/>
        IDictionary<string, int> Modifiers { get; }
    }

    #endregion Interface

    public class InPairUpFrontStatModifierEffect : SkillEffect, IInPairUpFrontStatModifierEffect
    {
        #region Attributes

        protected override string Name { get { return "InPairUpFrontStatModifier"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1/Param2. The unit stat modifiers to apply.
        /// </summary>
        public IDictionary<string, int> Modifiers { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public InPairUpFrontStatModifierEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.Modifiers = DataParser.StatValueCSVs_Int_Any(parameters, INDEX_PARAM_1, NAME_PARAM_1, INDEX_PARAM_2, NAME_PARAM_2);
        }

        /// <summary>
        /// Applies <c>Modifiers</c> to <paramref name="unit"/> if <paramref name="unit"/> has a paired partner.
        /// </summary>
        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            //Validate that the unit is in the front of a pairup
            if (unit.Location.PairedUnitObj == null || unit.Location.IsBackOfPair)
                return;

            unit.Stats.ApplyGeneralStatModifiers(this.Modifiers, skill.Name, true);
        }
    }
}
