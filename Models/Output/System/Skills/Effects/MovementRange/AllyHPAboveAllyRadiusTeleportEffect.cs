using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    #region Interface

    /// <inheritdoc cref="AllyHPAboveAllyRadiusTeleportEffect"/>
    public interface IAllyHPAboveAllyRadiusTeleportEffect
    {
        /// <inheritdoc cref="AllyHPAboveAllyRadiusTeleportEffect.TeleportationRange"/>
        int TeleportationRange { get; }

        /// <inheritdoc cref="AllyHPAboveAllyRadiusTeleportEffect.Radius"/>
        int Radius { get; }

        /// <inheritdoc cref="AllyHPAboveAllyRadiusTeleportEffect.HPPercentage"/>
        int HPPercentage { get; }
    }

    #endregion Interface

    public class AllyHPAboveAllyRadiusTeleportEffect : SkillEffect, IAllyHPAboveAllyRadiusTeleportEffect
    {
        #region Attributes

        protected override string Name { get { return "AllyHPAboveAllyRadiusTeleport"; } }

        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The distance within to search for allied units.
        /// </summary>
        public int TeleportationRange { get; private set; }

        /// <summary>
        /// Param2. The range within the unit can teleport adjacent to the ally unit.
        /// </summary>
        public int Radius { get; private set; }

        /// <summary>
        /// Param3. The minimum HP percentage the unit can have.
        /// </summary>
        public int HPPercentage { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public AllyHPAboveAllyRadiusTeleportEffect(List<string> parameters)
            : base(parameters)
        {
            this.TeleportationRange = DataParser.Int_NonZeroPositive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
            this.Radius = DataParser.Int_NonZeroPositive(parameters, INDEX_PARAM_2, NAME_PARAM_2);
            this.HPPercentage = DataParser.Int_Positive(parameters, INDEX_PARAM_3, NAME_PARAM_3);
        }

        /// <summary>
        /// Locates ally units and adds tiles within <c>Radius</c> tiles of their origin to <paramref name="unit"/>'s movement range.
        /// </summary>
        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            if (unit.Location.UnitSize > 1)
                throw new SkillEffectMultitileUnitsNotSupportedException(this.Name);

            //Locate valid ally units and select tiles near them
            List<ITile> tiles = units.Where(u => u.Name != unit.Name
                                              && u.Affiliation.Grouping == unit.Affiliation.Grouping
                                              && u.Stats.HP.Percentage >= this.HPPercentage
                                              && u.Location.IsOnMap()
                                              && (u.Location.OriginTiles.Any(o1 => unit.Location.OriginTiles.Any(o2 => o1.Coordinate.DistanceFrom(o2.Coordinate) <= this.TeleportationRange)) || this.TeleportationRange == 99))
                                     .SelectMany(u => map.GetTilesInRadius(u.Location.OriginTiles, this.Radius))
                                     .ToList();

            AddTeleportTargetsToUnitRange(unit, tiles);
        }
    }
}
