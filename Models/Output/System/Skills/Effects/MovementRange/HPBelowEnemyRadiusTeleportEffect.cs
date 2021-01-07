using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class HPBelowEnemyRadiusTeleportEffect : SkillEffect
    {
        #region Attributes

        protected override string SkillEffectName { get { return "HPBelowEnemyRadiusTeleport"; } }

        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The distance within to search for allied units.
        /// </summary>
        private int TeleportationRange { get; set; }

        /// <summary>
        /// Param2. The range within the unit can teleport adjacent to the enemy unit.
        /// </summary>
        private int Radius { get; set; }

        /// <summary>
        /// Param3. The maximum HP percentage the unit can have.
        /// </summary>
        private int HPPercentage { get; set; }

        #endregion

        public HPBelowEnemyRadiusTeleportEffect(IList<string> parameters)
            : base(parameters)
        {
            this.TeleportationRange = ParseHelper.SafeIntParse(parameters, 0, "Param1", true, true);
            this.Radius = ParseHelper.SafeIntParse(parameters, 1, "Param2", true, true);
            this.HPPercentage = ParseHelper.SafeIntParse(parameters, 2, "Param3", true);
        }

        /// <summary>
        /// Locates enemy units and adds tiles within <c>Radius</c> tiles of their origin to <paramref name="unit"/>'s movement range.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, IList<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (unit.OriginTile == null)
                return;

            //Unit must have an HP percentage below the configured value
            if (unit.HP.Percentage > this.HPPercentage)
                return;

            //Locate valid enemy units and select tiles near them
            IList<Tile> tiles = units.Where(u => u.AffiliationObj.Grouping != unit.AffiliationObj.Grouping
                                              && u.OriginTile != null
                                              && (u.OriginTile.Coordinate.DistanceFrom(unit.OriginTile.Coordinate) <= this.TeleportationRange || this.TeleportationRange == 99))
                                     .SelectMany(u => map.GetTilesInRadius(u.OriginTile, this.Radius))
                                     .ToList();

            AddTeleportTargetsToUnitRange(unit, tiles);
        }
    }
}
