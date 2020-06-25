using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public class RangeHelper
    {
        private IList<Unit> Units;
        private List<List<Tile>> Tiles;

        public RangeHelper(IList<Unit> units, List<List<Tile>> tiles)
        {
            this.Units = units;
            this.Tiles = tiles;
        }

        public void CalculateUnitRanges()
        {
            foreach(Unit unit in this.Units)
            {
                try
                {
                    //Ignore hidden units
                    if (unit.Coordinate.X < 1 || unit.Coordinate.Y < 1)
                        continue;


                    //Calculate movement range
                    UnitRangeParameters unitParms = new UnitRangeParameters(unit);
                    RecurseUnitRange(unitParms, unit.Stats["Mov"].FinalValue, unit.OriginTile.Coordinate, new List<Coordinate>());

                    //Find the items with minimum and maximum attack range
                    UnitInventoryItem minAtkRange = unit.Inventory.Where(i => i != null && i.CanEquip && i.Item.DealsDamage && i.Item.UtilizedStat.Length > 0).OrderBy(i => i.Item.Range.Minimum).FirstOrDefault();
                    UnitInventoryItem maxAtkRange = unit.Inventory.Where(i => i != null && i.CanEquip && i.Item.DealsDamage && i.Item.UtilizedStat.Length > 0).OrderByDescending(i => i.Item.Range.Maximum + i.MaxRangeModifier).FirstOrDefault();

                    //Find the items with minimum and maximum utility range
                    UnitInventoryItem minUtilRange = unit.Inventory.Where(i => i != null && i.CanEquip && !i.Item.DealsDamage && i.Item.UtilizedStat.Length > 0).OrderBy(i => i.Item.Range.Minimum).FirstOrDefault();
                    UnitInventoryItem maxUtilRange = unit.Inventory.Where(i => i != null && i.CanEquip && !i.Item.DealsDamage && i.Item.UtilizedStat.Length > 0).OrderByDescending(i => i.Item.Range.Maximum + i.MaxRangeModifier).FirstOrDefault();

                    IList<Coordinate> atkRange = new List<Coordinate>();
                    IList<Coordinate> utilRange = new List<Coordinate>();
                    foreach (Coordinate coord in unit.MovementRange)
                    {
                        //Calculate attack range
                        ItemRangeParameters atkParms = new ItemRangeParameters(coord, 
                            (minAtkRange != null ? minAtkRange.Item.Range.Minimum : 0), (maxAtkRange != null ? maxAtkRange.Item.Range.Maximum + maxAtkRange.MaxRangeModifier : 0));
                        RecurseItemRange( unit,
                                          atkParms,
                                          coord,
                                          atkParms.MinimumRange,
                                          atkParms.MaximumRange,
                                          new List<Coordinate>(),
                                          ref atkRange
                                        );

                        //Calculate utility range
                        ItemRangeParameters utilParms = new ItemRangeParameters(coord, 
                            (minUtilRange != null ? minUtilRange.Item.Range.Minimum : 0), (maxUtilRange != null ? maxUtilRange.Item.Range.Maximum + maxUtilRange.MaxRangeModifier : 0));
                        RecurseItemRange( unit,
                                          utilParms,
                                          coord,
                                          utilParms.MinimumRange,
                                          utilParms.MaximumRange,
                                          new List<Coordinate>(),
                                          ref utilRange
                                        );
                    }

                    unit.AttackRange = atkRange;
                    unit.UtilityRange = utilRange;
                }
                catch(Exception ex)
                {
                    throw new RangeCalculationException(unit, ex);
                }
            }
        }

        private void RecurseUnitRange(UnitRangeParameters unitParms, int remainingMov, Coordinate currCoord, List<Coordinate> visitedCoords)
        {
            //Base case
            //Don't exceed the maximum range and don't go off the map
            if (   remainingMov < 0
                || currCoord.X < 1
                || currCoord.Y < 1
                || currCoord.X > this.Tiles.First().Count
                || currCoord.Y > this.Tiles.Count
               )
                return;

            //Don't perform checks for the starting tile
            if(visitedCoords.Count > 0)
            {
                Tile tile = GetTileByCoord(currCoord);

                //If there is a Unit occupying this tile, check for affiliation collisions
                if (tile.Unit != null && unitParms.Unit.Name != tile.Unit.Name)
                {
                    if (!unitParms.IgnoresAffiliations && unitParms.Unit.AffiliationObj.Grouping != tile.Unit.AffiliationObj.Grouping)
                        return;
                }

                //Test that the unit can move to this tile
                int moveCost;
                if (!tile.TerrainTypeObj.MovementCosts.TryGetValue(unitParms.Unit.ClassList.First().MovementType, out moveCost))
                    throw new UnmatchedClassMovementTypeException(unitParms.Unit.ClassList.First().MovementType, tile.TerrainTypeObj.MovementCosts.Keys.ToList());

                //Apply movement cost modifiers
                TerrainTypeMovementCostSetEffect movCostSet = unitParms.MoveCostSets.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
                TerrainTypeMovementCostModifierEffect moveCostMod = unitParms.MovCostModifiers.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
                if (movCostSet != null && moveCost < 99)
                    moveCost = movCostSet.Value;
                else if (moveCostMod != null && moveCost < 99)
                    moveCost += moveCostMod.Value;

                if (moveCost < 0) moveCost = 0;
                if (moveCost > remainingMov || moveCost >= 99) return;
                remainingMov = remainingMov - moveCost;

            }

            visitedCoords.Add(currCoord);

            if (!unitParms.Unit.MovementRange.Contains(currCoord))
                unitParms.Unit.MovementRange.Add(currCoord);

            //Navigate in each cardinal direction, do not repeat tiles in this path
            //Left
            if(!visitedCoords.Contains(new Coordinate(currCoord.X - 1, currCoord.Y)))
                RecurseUnitRange(unitParms, remainingMov, new Coordinate(currCoord.X - 1, currCoord.Y), visitedCoords.ToList());

            //Right
            if (!visitedCoords.Contains(new Coordinate(currCoord.X + 1, currCoord.Y)))
                RecurseUnitRange(unitParms, remainingMov, new Coordinate(currCoord.X + 1, currCoord.Y), visitedCoords.ToList());

            //Up
            if (!visitedCoords.Contains(new Coordinate(currCoord.X, currCoord.Y - 1)))
                RecurseUnitRange(unitParms, remainingMov, new Coordinate(currCoord.X, currCoord.Y - 1), visitedCoords.ToList());

            //Down
            if (!visitedCoords.Contains(new Coordinate(currCoord.X, currCoord.Y + 1)))
                RecurseUnitRange(unitParms, remainingMov, new Coordinate(currCoord.X, currCoord.Y + 1), visitedCoords.ToList());

        }

        private void RecurseItemRange(Unit unit, ItemRangeParameters parms, Coordinate currCoord, int remainingMinRange, int remainingMaxRange, IList<Coordinate> visitedCoords, ref IList<Coordinate> itemRange)
        {
            //Base case
            //Don't exceed the maximum range and don't go off the map
            if (   remainingMaxRange < 0 
                || currCoord.X < 1
                || currCoord.Y < 1
                || currCoord.X > this.Tiles.First().Count
                || currCoord.Y > this.Tiles.Count
               )
                return;

            Tile tile = GetTileByCoord(currCoord);
            if (tile.TerrainTypeObj.BlocksItems) return;

            if (remainingMinRange <= 0)
            {
                visitedCoords.Add(currCoord);

                double originDisplacement = currCoord.DistanceFrom(parms.Origin);
                if (!unit.MovementRange.Contains(currCoord) && !itemRange.Contains(currCoord) && originDisplacement >= parms.MinimumRange && originDisplacement <= parms.MaximumRange)
                    itemRange.Add(currCoord);
            }

            //Navigate in each cardinal direction, do not repeat tiles in this path
            //Left
            if (!visitedCoords.Contains(new Coordinate(currCoord.X - 1, currCoord.Y)))
                RecurseItemRange(unit, parms, new Coordinate(currCoord.X - 1, currCoord.Y), remainingMinRange - 1, remainingMaxRange - 1, visitedCoords.ToList(), ref itemRange);

            //Right
            if (!visitedCoords.Contains(new Coordinate(currCoord.X + 1, currCoord.Y)))
                RecurseItemRange(unit, parms, new Coordinate(currCoord.X + 1, currCoord.Y), remainingMinRange - 1, remainingMaxRange - 1, visitedCoords.ToList(), ref itemRange);

            //Up
            if (!visitedCoords.Contains(new Coordinate(currCoord.X, currCoord.Y - 1)))
                RecurseItemRange(unit, parms, new Coordinate(currCoord.X, currCoord.Y - 1), remainingMinRange - 1, remainingMaxRange - 1, visitedCoords.ToList(), ref itemRange);

            //Down
            if (!visitedCoords.Contains(new Coordinate(currCoord.X, currCoord.Y + 1)))
                RecurseItemRange(unit, parms, new Coordinate(currCoord.X, currCoord.Y + 1), remainingMinRange - 1, remainingMaxRange - 1, visitedCoords.ToList(), ref itemRange);
        }

        /// <summary>
        /// Fetches the tile with matching coordinates to <paramref name="coord"/>.
        /// </summary>
        /// <exception cref="TileOutOfBoundsException"></exception>
        private Tile GetTileByCoord(Coordinate coord)
        {
            IList<Tile> row = this.Tiles.ElementAtOrDefault<IList<Tile>>(coord.Y - 1) ?? throw new TileOutOfBoundsException(coord.X, coord.Y);
            Tile column = row.ElementAtOrDefault<Tile>(coord.X - 1) ?? throw new TileOutOfBoundsException(coord.X, coord.Y);

            return column;
        }
    }

    public struct UnitRangeParameters
    {
        public Unit Unit;
        public bool IgnoresAffiliations { get; set; }
        public IList<TerrainTypeMovementCostModifierEffect> MovCostModifiers { get; set; }
        public IList<TerrainTypeMovementCostSetEffect> MoveCostSets { get; set; }

        public UnitRangeParameters(Unit unit)
        {
            this.Unit = unit;
            this.IgnoresAffiliations = unit.SkillList.Select(s => s.Effect).OfType<IgnoreUnitAffiliationsEffect>().Any();
            this.MovCostModifiers = unit.SkillList.Select(s => s.Effect).OfType<TerrainTypeMovementCostModifierEffect>().ToList();
            this.MoveCostSets = unit.SkillList.Select(s => s.Effect).OfType<TerrainTypeMovementCostSetEffect>().ToList();
        }
    }

    public struct ItemRangeParameters
    {
        public Coordinate Origin;
        public int MinimumRange;
        public int MaximumRange;

        public ItemRangeParameters(Coordinate origin, int minRange, int maxRange)
        {
            this.Origin = origin;
            this.MinimumRange = minRange;
            this.MaximumRange = maxRange;
        }
    }
}
