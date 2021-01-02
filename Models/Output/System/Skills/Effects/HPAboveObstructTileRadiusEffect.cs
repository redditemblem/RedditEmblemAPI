﻿using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class HPAboveObstructTileRadiusEffect : SkillEffect
    {
        #region Attributes

        protected override string SkillEffectName { get { return "HPAboveObstructTileRadius"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The range within this skill affects units.
        /// </summary>
        private int Radius { get; set; }

        /// <summary>
        /// Param2. The minimum HP percentage the unit can have.
        /// </summary>
        private int HPPercentage { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public HPAboveObstructTileRadiusEffect(IList<string> parameters)
            : base(parameters)
        {
            this.Radius = ParseHelper.SafeIntParse(parameters, 0, "Param1", true, true);
            this.HPPercentage = ParseHelper.SafeIntParse(parameters, 1, "Param2", true);
        }

        /// <summary>
        /// Marks all tiles within <c>this.Radius</c> tiles of <paramref name="unit"/>'s origin as obstructed when <paramref name="unit"/>'s HP percentage is above <c>this.HPPercentage</c>.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, IList<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (unit.OriginTile == null)
                return;

            //Unit HP percentage must be equal to or above the threshold
            if (unit.HP.Percentage < this.HPPercentage)
                return;

            List<Tile> radius = map.GetTilesInRadius(unit.OriginTile, this.Radius);
            radius.ForEach(t => t.ObstructingUnits.Add(unit));
        }
    }
}
