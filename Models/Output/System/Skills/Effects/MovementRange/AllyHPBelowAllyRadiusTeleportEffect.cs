using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class AllyHPBelowAllyRadiusTeleportEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "AllyHPBelowAllyRadiusTeleport"; } }

        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The distance within to search for allied units.
        /// </summary>
        private int TeleportationRange { get; set; }

        /// <summary>
        /// Param2. The range within the unit can teleport adjacent to the ally unit.
        /// </summary>
        private int Radius { get; set; }

        /// <summary>
        /// Param3. The minimum HP percentage the unit can have.
        /// </summary>
        private int HPPercentage { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public AllyHPBelowAllyRadiusTeleportEffect(List<string> parameters)
            : base(parameters)
        {
            this.TeleportationRange = DataParser.Int_NonZeroPositive(parameters, 0, "Param1");
            this.Radius = DataParser.Int_NonZeroPositive(parameters, 1, "Param2");
            this.HPPercentage = DataParser.Int_Positive(parameters, 2, "Param3");
        }

        /// <summary>
        /// Locates ally units and adds tiles within <c>Radius</c> tiles of their origin to <paramref name="unit"/>'s movement range.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            if (unit.Location.UnitSize > 1)
                throw new SkillEffectMultitileUnitsNotSupportedException(this.Name);

            //Locate valid ally units and select tiles near them
            List<Tile> tiles = units.Where(u => u.Name != unit.Name
                                              && u.AffiliationObj.Grouping == unit.AffiliationObj.Grouping
                                              && u.Stats.HP.Percentage <= this.HPPercentage
                                              && u.Location.IsOnMap()
                                              && (u.Location.OriginTiles.Any(o1 => unit.Location.OriginTiles.Any(o2 => o1.Coordinate.DistanceFrom(o2.Coordinate) <= this.TeleportationRange)) || this.TeleportationRange == 99))
                                     .SelectMany(u => map.GetTilesInRadius(u.Location.OriginTiles, this.Radius))
                                     .ToList();

            AddTeleportTargetsToUnitRange(unit, tiles);
        }
    }
}
