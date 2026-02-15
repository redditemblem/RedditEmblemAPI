using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

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
        public ObstructTileRadiusEffect(List<string> parameters)
            : base(parameters)
        {
            this.Radius = DataParser.Int_NonZeroPositive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
        }

        /// <summary>
        /// Marks all tiles within <c>this.Radius</c> tiles of <paramref name="unit"/>'s origin as obstructed.
        /// </summary>
        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            //If unit is not on the map, don't apply
            if (!unit.Location.IsOnMap())
                return;

            List<ITile> radius = map.GetTilesInRadius(unit.Location.OriginTiles, this.Radius);
            radius.ForEach(t => t.UnitData.UnitsObstructingMovement.Add(unit));
        }
    }
}
