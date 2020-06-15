using RedditEmblemAPI.Models.Common;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output;
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

        public void CalculateUnitRange()
        {
            foreach(Unit unit in this.Units)
            {
                try
                {
                    //Ignore hidden units
                    if (unit.Coordinates.X < 1 || unit.Coordinates.Y < 1)
                        continue;

                    //Calculate movement range
                    RecurseUnitRange(unit, unit.Stats["Mov"].FinalValue, unit.OriginTile.Coordinate, new List<Coordinate>());

                    //Find the items with minimum and maximum attack range
                    UnitHeldItem minAtkRange = unit.Inventory.Where(i => i != null && i.CanEquip && i.Item.DealsDamage && i.Item.UtilizedStat.Length > 0).OrderBy(i => i.Item.Range.Minimum).FirstOrDefault();
                    UnitHeldItem maxAtkRange = unit.Inventory.Where(i => i != null && i.CanEquip && i.Item.DealsDamage && i.Item.UtilizedStat.Length > 0).OrderByDescending(i => i.Item.Range.Maximum).FirstOrDefault();

                    //Find the items with minimum and maximum utility range
                    UnitHeldItem minUtilRange = unit.Inventory.Where(i => i != null && i.CanEquip && !i.Item.DealsDamage && i.Item.UtilizedStat.Length > 0).OrderBy(i => i.Item.Range.Minimum).FirstOrDefault();
                    UnitHeldItem maxUtilRange = unit.Inventory.Where(i => i != null && i.CanEquip && !i.Item.DealsDamage && i.Item.UtilizedStat.Length > 0).OrderByDescending(i => i.Item.Range.Maximum).FirstOrDefault();

                    IList<Coordinate> atkRange = new List<Coordinate>();
                    IList<Coordinate> utilRange = new List<Coordinate>();
                    foreach (Coordinate coord in unit.MovementRange)
                    {
                        //Calculate attack range
                        ItemRangeParameters atkParms = new ItemRangeParameters(coord, (minAtkRange != null ? minAtkRange.Item.Range.Minimum : 0), (maxAtkRange != null ? maxAtkRange.Item.Range.Maximum : 0));
                        RecurseItemRange( unit,
                                          atkParms,
                                          coord,
                                          atkParms.MinimumRange,
                                          atkParms.MaximumRange,
                                          new List<Coordinate>(),
                                          ref atkRange
                                        );

                        //Calculate utility range
                        ItemRangeParameters utilParms = new ItemRangeParameters(coord, (minUtilRange != null ? minUtilRange.Item.Range.Minimum : 0), (maxUtilRange != null ? maxUtilRange.Item.Range.Maximum : 0));
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
                    throw new RangeCalculationException(unit.Name, ex);
                }
            }
        }

        private void RecurseUnitRange(Unit unit, int remainingMov, Coordinate currCoord, List<Coordinate> visitedCoords)
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

                //Test that the unit can move to this tile
                int moveCost;
                if (!tile.TerrainTypeObj.MovementCosts.TryGetValue(unit.ClassList.First().MovementType, out moveCost))
                    throw new UnmatchedMovementTypeException(unit.ClassList.First().MovementType, tile.TerrainTypeObj.MovementCosts.Keys);
                if (moveCost > remainingMov || moveCost >= 99) return;
                remainingMov = remainingMov - moveCost;

                //If there is a Unit occupying this tile, check for affiliation collisions
                if (tile.Unit != null && unit.Name != tile.Unit.Name)
                {
                    if (unit.AffiliationObj.Grouping != tile.Unit.AffiliationObj.Grouping)
                        return;
                }
            }

            visitedCoords.Add(currCoord);

            if (!unit.MovementRange.Contains(currCoord))
                unit.MovementRange.Add(currCoord);

            //Navigate in each cardinal direction, do not repeat tiles in this path
            //Left
            if(!visitedCoords.Contains(new Coordinate(currCoord.X - 1, currCoord.Y)))
                RecurseUnitRange(unit, remainingMov, new Coordinate(currCoord.X - 1, currCoord.Y), visitedCoords.ToList());

            //Right
            if (!visitedCoords.Contains(new Coordinate(currCoord.X + 1, currCoord.Y)))
                RecurseUnitRange(unit, remainingMov, new Coordinate(currCoord.X + 1, currCoord.Y), visitedCoords.ToList());

            //Up
            if (!visitedCoords.Contains(new Coordinate(currCoord.X, currCoord.Y - 1)))
                RecurseUnitRange(unit, remainingMov, new Coordinate(currCoord.X, currCoord.Y - 1), visitedCoords.ToList());

            //Down
            if (!visitedCoords.Contains(new Coordinate(currCoord.X, currCoord.Y + 1)))
                RecurseUnitRange(unit, remainingMov, new Coordinate(currCoord.X, currCoord.Y + 1), visitedCoords.ToList());

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

        private Tile GetTileByCoord(Coordinate coord)
        {
            IList<Tile> row = this.Tiles.ElementAtOrDefault(coord.Y - 1);
            if (row == null) return null;

            return row.ElementAtOrDefault(coord.X - 1);
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
