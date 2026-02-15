using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.Radius
{
    public class NoAllyRadiusSelfCombatStatModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "NoAllyRadiusSelfCombatStatModifier"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The range within this skill affects units.
        /// </summary>
        private int Radius { get; set; }

        /// <summary>
        /// Param2/Param3. The unit combat stat modifiers to apply.
        /// </summary>
        private IDictionary<string, int> Modifiers { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public NoAllyRadiusSelfCombatStatModifierEffect(List<string> parameters)
            : base(parameters)
        {
            this.Radius = DataParser.Int_NonZeroPositive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
            this.Modifiers = DataParser.StatValueCSVs_Int_Any(parameters, INDEX_PARAM_2, NAME_PARAM_2, INDEX_PARAM_3, NAME_PARAM_3);
        }

        /// <summary>
        /// Applies <c>Modifiers</c> to <paramref name="unit"/> if a friendly, allied unit does not exist within <c>Radius</c> tiles of <paramref name="unit"/>.
        /// </summary>
        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            //If there are no allies in range, apply modifiers
            if (!units.Any(u => u.Name != unit.Name //different unit name
                             && u.Affiliation.Grouping == unit.Affiliation.Grouping //same affiliation grouping
                             && u.Location.IsOnMap()
                             && u.Location.OriginTiles.Any(o1 => unit.Location.OriginTiles.Any(o2 => o2.Coordinate.DistanceFrom(o1.Coordinate) <= this.Radius))))
            {
                unit.Stats.ApplyCombatStatModifiers(this.Modifiers, skill.Name, true);
            }
        }
    }
}