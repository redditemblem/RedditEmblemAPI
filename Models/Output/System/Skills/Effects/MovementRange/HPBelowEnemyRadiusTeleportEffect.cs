﻿using RedditEmblemAPI.Models.Exceptions.Validation;
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

        protected override string Name { get { return "HPBelowEnemyRadiusTeleport"; } }

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
            this.TeleportationRange = ParseHelper.Int_NonZeroPositive(parameters, 0, "Param1");
            this.Radius = ParseHelper.Int_NonZeroPositive(parameters, 1, "Param2");
            this.HPPercentage = ParseHelper.Int_Positive(parameters, 2, "Param3");
        }

        /// <summary>
        /// Locates enemy units and adds tiles within <c>Radius</c> tiles of their origin to <paramref name="unit"/>'s movement range.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, IList<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (unit.OriginTiles.Count == 0)
                return;

            //Unit must have an HP percentage below the configured value
            if (unit.HP.Percentage > this.HPPercentage)
                return;

            if (unit.UnitSize > 1)
                throw new SkillEffectMultitileUnitsNotSupportedException(this.Name);

            //Locate valid enemy units and select tiles near them
            IList<Tile> tiles = units.Where(u => u.AffiliationObj.Grouping != unit.AffiliationObj.Grouping
                                              && u.OriginTiles.Count > 0
                                              && (u.OriginTiles.Any(o1 => unit.OriginTiles.Any(o2 => o1.Coordinate.DistanceFrom(o2.Coordinate) <= this.TeleportationRange)) || this.TeleportationRange == 99))
                                     .SelectMany(u => map.GetTilesInRadius(u.OriginTiles, this.Radius))
                                     .ToList();

            AddTeleportTargetsToUnitRange(unit, tiles);
        }
    }
}
