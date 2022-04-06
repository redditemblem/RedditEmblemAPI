using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

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
        public HPAboveObstructTileRadiusEffect(IList<string> parameters)
            : base(parameters)
        {
            this.Radius = DataParser.Int_NonZeroPositive(parameters, 0, "Param1");
            this.HPPercentage = DataParser.Int_Positive(parameters, 1, "Param2");
        }

        /// <summary>
        /// Marks all tiles within <c>this.Radius</c> tiles of <paramref name="unit"/>'s origin as obstructed when <paramref name="unit"/>'s HP percentage is above <c>this.HPPercentage</c>.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, IList<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            //Unit HP percentage must be equal to or above the threshold
            if (unit.Stats.HP.Percentage < this.HPPercentage)
                return;

            List<Tile> radius = map.GetTilesInRadius(unit.Location.OriginTiles, this.Radius);
            radius.ForEach(t => t.UnitData.UnitsObstructingMovement.Add(unit));
        }
    }
}
