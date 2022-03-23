using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class RadiusAllyMovementCostSetEffect : SkillEffect, IAffectMovementCost
    {
        #region Attributes

        protected override string Name { get { return "RadiusAllyMovementCostSet"; } }

        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The range within this skill affects units.
        /// </summary>
        public int Radius { get; set; }

        /// <summary>
        /// Param2. The movement cost for allies to move through tiles occupied by a unit with this skill.
        /// </summary>
        public int MovementCost { get; set; }

        #endregion

        public RadiusAllyMovementCostSetEffect(IList<string> parameters)
            : base(parameters)
        {
            this.Radius = ParseHelper.Int_Positive(parameters, 0, "Param1");
            this.MovementCost = ParseHelper.Int_Positive(parameters, 1, "Param2");
        }

        public override void Apply(Unit unit, Skill skill, MapObj map, IList<Unit> units)
        {
            //Ignore units not on map
            if (!unit.Location.IsOnMap())
                return;

            List<Tile> radius = map.GetTilesInRadius(unit.Location.OriginTiles, this.Radius).Union(unit.Location.OriginTiles).ToList();
            radius.ForEach(t => t.UnitData.UnitsAffectingMovementCosts.Add(unit));
        }

        public bool IsActive(Unit tileUnit, Unit movingUnit)
        {
            //Return true when the units are in the same grouping
            return tileUnit.AffiliationObj.Grouping == movingUnit.AffiliationObj.Grouping;
        }

        public int GetMovementCost()
        {
            return this.MovementCost;
        }
    }
}
