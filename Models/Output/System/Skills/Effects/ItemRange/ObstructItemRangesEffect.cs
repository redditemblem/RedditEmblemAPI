using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange
{
    public class ObstructItemRangesEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "ObstructItemRanges"; } }
        protected override int ParameterCount { get { return 1; } }

        /// <summary>
        /// Param1. The range within this skill affects item ranges.
        /// </summary>
        private int Radius { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public ObstructItemRangesEffect(List<string> parameters)
            : base(parameters)
        {
            this.Radius = DataParser.Int_Positive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
        }

        /// <summary>
        /// ddfdf
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            //Ignore units not on the map
            if (!unit.Location.IsOnMap())
                return;

            List<Tile> radius = map.GetTilesInRadius(unit.Location.OriginTiles, this.Radius).Union(unit.Location.OriginTiles).ToList();
            radius.ForEach(t => t.UnitData.UnitsObstructingItems.Add(unit));
        }
    }
}
