using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class HPAboveObstructTileRadiusEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "HPAboveObstructTileRadius"; } }
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
        public HPAboveObstructTileRadiusEffect(List<string> parameters)
            : base(parameters)
        {
            this.Radius = DataParser.Int_NonZeroPositive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
            this.HPPercentage = DataParser.Int_Positive(parameters, INDEX_PARAM_2, NAME_PARAM_2);
        }

        /// <summary>
        /// Marks all tiles within <c>this.Radius</c> tiles of <paramref name="unit"/>'s origin as obstructed when <paramref name="unit"/>'s HP percentage is above <c>this.HPPercentage</c>.
        /// </summary>
        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            //Unit HP percentage must be equal to or above the threshold
            if (unit.Stats.HP.Percentage < this.HPPercentage)
                return;

            IEnumerable<ITile> radius = map.GetTilesInRadius(unit.Location.OriginTiles, this.Radius);
            foreach(ITile tile in radius)
                tile.UnitData.UnitsObstructingMovement.Add(unit);
        }
    }
}
