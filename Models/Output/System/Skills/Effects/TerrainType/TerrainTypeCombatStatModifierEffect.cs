using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.TerrainType
{
    public class TerrainTypeCombatStatModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "TerrainTypeCombatStatModifier"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The terrain type grouping to look for <c>Tile</c>s in.
        /// </summary>
        private int TerrainTypeGrouping { get; set; }

        /// <summary>
        /// Param2/Param3. The unit combat stat modifiers to apply.
        /// </summary>
        private IDictionary<string, int> Modifiers { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public TerrainTypeCombatStatModifierEffect(List<string> parameters)
            : base(parameters)
        {
            this.TerrainTypeGrouping = DataParser.Int_Positive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
            this.Modifiers = DataParser.StatValueCSVs_Int_Any(parameters, INDEX_PARAM_2, NAME_PARAM_2, INDEX_PARAM_3, NAME_PARAM_3);
        }

        /// <summary>
        /// Applies <c>Modifiers</c> to <paramref name="unit"/> if <paramref name="unit"/> originates on a tile with a terrain type in <c>TerrainTypeGrouping</c>.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            //The terrain type must be in the defined grouping
            if (!unit.Location.OriginTiles.Any(o => o.TerrainTypeObj.Groupings.Contains(this.TerrainTypeGrouping)))
                return;

            unit.Stats.ApplyCombatStatModifiers(this.Modifiers, skill.Name);
        }
    }
}
