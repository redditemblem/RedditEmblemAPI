﻿using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.TerrainType
{
    public class TerrainTypeCombatStatBonusStatModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "TerrainTypeCombatStatBonusStatModifier"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The terrain type combat stat modifier to evaluate. 
        /// </summary>
        private string TerrainTypeStat { get; set; }

        /// <summary>
        /// Param2/Param3. The unit stat modifiers to apply.
        /// </summary>
        private IDictionary<string, int> Modifiers { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public TerrainTypeCombatStatBonusStatModifierEffect(List<string> parameters)
            : base(parameters)
        {
            this.TerrainTypeStat = DataParser.String(parameters, INDEX_PARAM_1, NAME_PARAM_1);
            this.Modifiers = DataParser.StatValueCSVs_Int_Any(parameters, INDEX_PARAM_2, NAME_PARAM_2, INDEX_PARAM_3, NAME_PARAM_3);
        }

        /// <summary>
        /// Applies <c>Modifiers</c> to <paramref name="unit"/> if <paramref name="unit"/> is standing on terrain that grants a positive modifier to <c>this.TerrainTypeStat</c>.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, IReadOnlyCollection<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            foreach(Tile tile in unit.Location.OriginTiles)
            {
                TerrainTypeStats stats = tile.TerrainTypeObj.GetTerrainTypeStatsByAffiliation(unit.AffiliationObj);

                int modifier;
                if (!stats.CombatStatModifiers.TryGetValue(this.TerrainTypeStat, out modifier))
                    continue;

                //Modifier must be positive
                if (modifier <= 0)
                    continue;

                unit.Stats.ApplyGeneralStatModifiers(this.Modifiers, skill.Name);
                break;
            }
            
        }
    }
}
