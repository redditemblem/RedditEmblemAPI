using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class ObstructTileRadiusEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "ObstructTileRadius"; } }
        protected override int ParameterCount { get { return 1; } }

        /// <summary>
        /// Param1. The range within this skill affects units.
        /// </summary>
        private int Radius { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public ObstructTileRadiusEffect(IList<string> parameters)
            : base(parameters)
        {
            this.Radius = ParseHelper.Int_NonZeroPositive(parameters, 0, "Param1");
        }

        /// <summary>
        /// Marks all tiles within <c>this.Radius</c> tiles of <paramref name="unit"/>'s origin as obstructed.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, IList<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            List<Tile> radius = map.GetTilesInRadius(unit.Location.OriginTiles, this.Radius);
            radius.ForEach(t => t.UnitData.ObstructingUnits.Add(unit));
        }
    }
}
