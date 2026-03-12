using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange
{
    #region Interface

    /// <inheritdoc cref="ObstructItemRangesEffect"/>
    public interface IObstructItemRangesEffect
    {
        /// <inheritdoc cref="ObstructItemRangesEffect.Radius"/>
        int Radius { get; }
    }

    #endregion Interface

    public class ObstructItemRangesEffect : SkillEffect, IObstructItemRangesEffect
    {
        #region Attributes

        protected override string Name { get { return "ObstructItemRanges"; } }
        protected override int ParameterCount { get { return 1; } }

        /// <summary>
        /// Param1. The range within this skill affects item ranges.
        /// </summary>
        public int Radius { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public ObstructItemRangesEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.Radius = DataParser.Int_Positive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
        }

        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            //Ignore units not on the map
            if (!unit.Location.IsOnMap())
                return;

            List<ITile> radius = map.GetTilesInRadius(unit.Location.OriginTiles, this.Radius).Union(unit.Location.OriginTiles).ToList();
            radius.ForEach(t => t.UnitData.UnitsObstructingItems.Add(unit));
        }
    }
}
