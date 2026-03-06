using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.PairUp
{
    public class InPairUpBackAllyStatModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "InPairUpBackAllyStatModifier"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1/Param2. The unit stat modifiers to apply.
        /// </summary>
        private IDictionary<string, int> Modifiers { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public InPairUpBackAllyStatModifierEffect(List<string> parameters)
            : base(parameters)
        {
            this.Modifiers = DataParser.StatValueCSVs_Int_Any(parameters, INDEX_PARAM_1, NAME_PARAM_1, INDEX_PARAM_2, NAME_PARAM_2);
        }

        /// <summary>
        /// Applies <c>Modifiers</c> to <paramref name="unit"/>'s paired partner, if one exists.
        /// </summary>
        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            //Validate that the unit is in the back of a pairup
            if (unit.Location.PairedUnitObj == null || !unit.Location.IsBackOfPair)
                return;

            unit.Location.PairedUnitObj.Stats.ApplyGeneralStatModifiers(this.Modifiers, $"{unit.Name}'s {skill.Name}", true);
        }
    }
}
