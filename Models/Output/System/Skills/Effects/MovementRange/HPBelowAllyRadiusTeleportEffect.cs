using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class HPBelowAllyRadiusTeleportEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "HPBelowAllyRadiusTeleport"; } }

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
        /// Param3. The maximum HP percentage the unit can have.
        /// </summary>
        private int HPPercentage { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public HPBelowAllyRadiusTeleportEffect(List<string> parameters)
            : base(parameters)
        {
            this.TeleportationRange = DataParser.Int_NonZeroPositive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
            this.Radius = DataParser.Int_NonZeroPositive(parameters, INDEX_PARAM_2, NAME_PARAM_2);
            this.HPPercentage = DataParser.Int_Positive(parameters, INDEX_PARAM_3, NAME_PARAM_3);
        }

        /// <summary>
        /// Locates ally units and adds tiles within <c>Radius</c> tiles of their origin to <paramref name="unit"/>'s movement range.
        /// </summary>
        public override void Apply(Unit unit, ISkill skill, MapObj map, List<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            //Unit must have an HP percentage below the configured value
            if (unit.Stats.HP.Percentage > this.HPPercentage)
                return;

            if (unit.Location.UnitSize > 1)
                throw new SkillEffectMultitileUnitsNotSupportedException(this.Name);

            //Locate valid ally units and select tiles near them
            List<Tile> tiles = units.Where(u => u.Name != unit.Name
                                              && u.AffiliationObj.Grouping == unit.AffiliationObj.Grouping
                                              && u.Location.IsOnMap()
                                              && (u.Location.OriginTiles.Any(o1 => unit.Location.OriginTiles.Any(o2 => o1.Coordinate.DistanceFrom(o2.Coordinate) <= this.TeleportationRange)) || this.TeleportationRange == 99))
                                     .SelectMany(u => map.GetTilesInRadius(u.Location.OriginTiles, this.Radius))
                                     .ToList();

            AddTeleportTargetsToUnitRange(unit, tiles);
        }
    }
}
