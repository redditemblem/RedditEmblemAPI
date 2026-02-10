using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.Radius
{
    public class AllyRadiusStatModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "AllyRadiusStatModifier"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The range within this skill affects units.
        /// </summary>
        private int Radius { get; set; }

        /// <summary>
        /// Param2/Param3. The unit stat modifiers to apply.
        /// </summary>
        private IDictionary<string, int> Modifiers { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public AllyRadiusStatModifierEffect(List<string> parameters)
            : base(parameters)
        {
            this.Radius = DataParser.Int_NonZeroPositive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
            this.Modifiers = DataParser.StatValueCSVs_Int_Any(parameters, INDEX_PARAM_2, NAME_PARAM_2, INDEX_PARAM_3, NAME_PARAM_3);
        }

        /// <summary>
        /// Applies <c>Modifiers</c> to all friendly, allied units within <c>Radius</c> tiles of <paramref name="unit"/>.
        /// </summary>
        public override void Apply(Unit unit, ISkill skill, MapObj map, List<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            //Apply modifiers to allies in range
            foreach (Unit ally in units.Where(u => u.Name != unit.Name //different names
                                                && u.AffiliationObj.Grouping == unit.AffiliationObj.Grouping //same affiliation grouping
                                                && u.Location.IsOnMap()
                                                && u.Location.OriginTiles.Any(o1 => unit.Location.OriginTiles.Any(o2 => o2.Coordinate.DistanceFrom(o1.Coordinate) <= this.Radius))))
            {
                ally.Stats.ApplyGeneralStatModifiers(this.Modifiers, $"{unit.Name}'s {skill.Name}", true);
            }
        }
    }
}
