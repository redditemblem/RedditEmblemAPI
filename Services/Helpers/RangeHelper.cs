using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System;
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
                    RecurseUnitRange(unitParms, unit.OriginTile.Coordinate, unit.Stats["Mov"].FinalValue, string.Empty);

                    //Calculate item range
                    IList<Coordinate> atkRange = new List<Coordinate>();
                    IList<Coordinate> utilRange = new List<Coordinate>();

                    IList<UnitItemRange> itemRanges = unit.Inventory.Where(i => i != null && i.CanEquip && i.Item.UtilizedStats.Any() && (i.ModifiedMinRangeValue > 0 || i.ModifiedMaxRangeValue > 0))
                                                                    .Select(i => new UnitItemRange(i.ModifiedMinRangeValue, i.ModifiedMaxRangeValue, i.Item.DealsDamage))
                                                                    .ToList();
                    if (itemRanges.Any())
                    {
                        foreach (Coordinate coord in unit.MovementRange)
                        {
                            //Calculate attack range
                            ItemRangeParameters rangeParms = new ItemRangeParameters(unit, coord, itemRanges);
                            RecurseItemRange(rangeParms,
                                             coord,
                                             rangeParms.LargestRange,
                                             string.Empty,
                                             ref atkRange,
                                             ref utilRange
                                            );
                        }
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

        private void RecurseUnitRange(UnitRangeParameters parms, Coordinate currCoord, int remainingMov, string visitedCoords)
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
            if(visitedCoords.Length > 0)
            {
                Tile tile = GetTileByCoord(currCoord);

                //If there is a Unit occupying this tile, check for affiliation collisions
                if (tile.Unit != null && parms.Unit.Name != tile.Unit.Name)
                {
                    if (!parms.IgnoresAffiliations && parms.Unit.AffiliationObj.Grouping != tile.Unit.AffiliationObj.Grouping)
                        return;
                }

                //Test that the unit can move to this tile
                int moveCost;
                if (!tile.TerrainTypeObj.MovementCosts.TryGetValue(parms.Unit.GetUnitMovementType(), out moveCost))
                    throw new UnmatchedMovementTypeException(parms.Unit.GetUnitMovementType(), tile.TerrainTypeObj.MovementCosts.Keys.ToList());

                //Apply movement cost modifiers
                TerrainTypeMovementCostSetEffect movCostSet = parms.MoveCostSets.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
                TerrainTypeMovementCostModifierEffect moveCostMod = parms.MovCostModifiers.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
                if (movCostSet != null && (movCostSet.CanOverride99MoveCost || moveCost < 99))
                    moveCost = movCostSet.Value;
                else if (moveCostMod != null && moveCost < 99)
                    moveCost += moveCostMod.Value;

                if (moveCost < 0) moveCost = 0;
                if (moveCost > remainingMov || moveCost >= 99) return;
                if (parms.Unit.UnitSize > 1 && !UnitCanAccessAllIntersectedTiles(parms, tile))
                    return;
               
                remainingMov = remainingMov - moveCost;
            }

            visitedCoords += "_" + currCoord.ToString() + "_";

            if (!parms.Unit.MovementRange.Contains(currCoord))
                parms.Unit.MovementRange.Add(currCoord);

            //Navigate in each cardinal direction, do not repeat tiles in this path
            //Left
            Coordinate left = new Coordinate(currCoord.X - 1, currCoord.Y);
            if (!visitedCoords.Contains("_" + left.ToString() + "_"))
                RecurseUnitRange(parms, left, remainingMov, visitedCoords);

            //Right
            Coordinate right = new Coordinate(currCoord.X + 1, currCoord.Y);
            if (!visitedCoords.Contains("_" + right.ToString() + "_"))
                RecurseUnitRange(parms, right, remainingMov, visitedCoords);

            //Up
            Coordinate up = new Coordinate(currCoord.X, currCoord.Y - 1);
            if (!visitedCoords.Contains("_" + up.ToString() + "_"))
                RecurseUnitRange(parms, up, remainingMov, visitedCoords);

            //Down
            Coordinate down = new Coordinate(currCoord.X, currCoord.Y + 1);
            if (!visitedCoords.Contains("_" + down.ToString() + "_"))
                RecurseUnitRange(parms, down, remainingMov, visitedCoords);

        }

        private void RecurseItemRange(ItemRangeParameters parms, Coordinate currCoord, int remainingRange, string visitedCoords, ref IList<Coordinate> atkRange, ref IList<Coordinate> utilRange)
        {
            //Base case
            //Don't exceed the maximum range and don't go off the map
            if (   remainingRange < 0 
                || currCoord.X < 1
                || currCoord.Y < 1
                || currCoord.X > this.Tiles.First().Count
                || currCoord.Y > this.Tiles.Count
               )
                return;

            Tile tile = GetTileByCoord(currCoord);

            //Check if ranges can pass through this tile
            if (tile.TerrainTypeObj.BlocksItems)
                return;

            visitedCoords += "_" + currCoord.ToString() + "_";

            //Check for items that can reach this tile
            if (!parms.Unit.MovementRange.Contains(currCoord))
            {
                int displacement = currCoord.DistanceFrom(parms.StartCoord);
                int pathLength = parms.LargestRange - remainingRange;

                IList<UnitItemRange> validRanges = parms.Ranges.Where(r => r.MinRange <= displacement //tile greater than min range away from unit
                                                                        && r.MinRange <= pathLength //tile greater than min range down the path
                                                                        && r.MaxRange >= displacement //tile less than max range from unit
                                                                        && r.MaxRange >= pathLength) //tile less than max range down path
                                                               .ToList();
                //Add to attacking range
                if (validRanges.Any(r => r.DealsDamage) && !atkRange.Contains(currCoord))
                    atkRange.Add(currCoord);
                //Add to util range
                else if (validRanges.Any(r => !r.DealsDamage) && !utilRange.Contains(currCoord))
                    utilRange.Add(currCoord);
            }

            //Navigate in each cardinal direction, do not repeat tiles in this path
            //Left
            Coordinate left = new Coordinate(currCoord.X - 1, currCoord.Y);
            if (!visitedCoords.Contains("_" + left.ToString() + "_"))
                RecurseItemRange(parms, left, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);

            //Right
            Coordinate right = new Coordinate(currCoord.X + 1, currCoord.Y);
            if (!visitedCoords.Contains("_" + right.ToString() + "_"))
                RecurseItemRange(parms, right, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);

            //Up
            Coordinate up = new Coordinate(currCoord.X, currCoord.Y - 1);
            if (!visitedCoords.Contains("_" + up.ToString() + "_"))
                RecurseItemRange(parms, up, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);

            //Down
            Coordinate down = new Coordinate(currCoord.X, currCoord.Y + 1);
            if (!visitedCoords.Contains("_" + down.ToString() + "_"))
                RecurseItemRange(parms, down, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);
        }

        private bool UnitCanAccessAllIntersectedTiles(UnitRangeParameters unitParms, Tile currentOriginTile)
        {
            int anchorOffset = (int)Math.Ceiling(unitParms.Unit.UnitSize / 2.0m) - 1;

            //Make sure the relative anchor doesn't fall off the map
            if( currentOriginTile.Coordinate.X - anchorOffset < 1
             || currentOriginTile.Coordinate.Y - anchorOffset < 1
             || currentOriginTile.Coordinate.X - anchorOffset > this.Tiles.First().Count
             || currentOriginTile.Coordinate.Y - anchorOffset > this.Tiles.Count
              )
                return false;

            Tile relativeAnchorTile = GetTileByCoord(currentOriginTile.Coordinate.X - anchorOffset, currentOriginTile.Coordinate.Y - anchorOffset);

            for (int y = 0; y < unitParms.Unit.UnitSize; y++)
            {
                for (int x = 0; x < unitParms.Unit.UnitSize; x++)
                {
                    //We don't need to recheck the current origin
                    if (currentOriginTile.Coordinate.X == relativeAnchorTile.Coordinate.X + x && currentOriginTile.Coordinate.Y == relativeAnchorTile.Coordinate.Y + y)
                        continue;

                    Tile tile = GetTileByCoord(relativeAnchorTile.Coordinate.X + x, relativeAnchorTile.Coordinate.Y + y);

                    //If there is a Unit occupying this tile, check for affiliation collisions
                    if (tile.Unit != null && unitParms.Unit.Name != tile.Unit.Name)
                    {
                        if (!unitParms.IgnoresAffiliations && unitParms.Unit.AffiliationObj.Grouping != tile.Unit.AffiliationObj.Grouping)
                            return false;
                    }

                    int moveCost;
                    if (!tile.TerrainTypeObj.MovementCosts.TryGetValue(unitParms.Unit.GetUnitMovementType(), out moveCost))
                        throw new UnmatchedMovementTypeException(unitParms.Unit.GetUnitMovementType(), tile.TerrainTypeObj.MovementCosts.Keys.ToList());

                    //We only care if the unit cannot move onto this tile at all
                    //Move costs only matters for the origin
                    if (moveCost >= 99)
                        return false;
                }
            }

            return true;
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


        /// <summary>
        /// Fetches the tile with matching coordinates to <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        /// <exception cref="TileOutOfBoundsException"></exception>
        private Tile GetTileByCoord(int x, int y)
        {
            IList<Tile> row = this.Tiles.ElementAtOrDefault<IList<Tile>>(y - 1) ?? throw new TileOutOfBoundsException(x, y);
            Tile column = row.ElementAtOrDefault<Tile>(x - 1) ?? throw new TileOutOfBoundsException(x, y);

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
            this.IgnoresAffiliations = unit.SkillList.Select(s => s.Effect).OfType<IIgnoreUnitAffiliations>().Any(e => e.IsActive(unit));
            this.MovCostModifiers = unit.SkillList.Select(s => s.Effect).OfType<TerrainTypeMovementCostModifierEffect>().ToList();
            this.MoveCostSets = unit.SkillList.Select(s => s.Effect).OfType<TerrainTypeMovementCostSetEffect>().ToList();
        }
    }

    public struct ItemRangeParameters
    {
        public Unit Unit;
        public Coordinate StartCoord;
        public IList<UnitItemRange> Ranges;
        public int LargestRange;

        public ItemRangeParameters(Unit unit, Coordinate startCoord, IList<UnitItemRange> ranges)
        {
            this.Unit = unit;
            this.StartCoord = startCoord;
            this.Ranges = ranges;
            this.LargestRange = this.Ranges.Select(r => r.MaxRange).OrderByDescending(r => r).FirstOrDefault();
        }
    }

    public struct UnitItemRange
    {
        public int MinRange;
        public int MaxRange;
        public bool DealsDamage;

        public UnitItemRange(int minRange, int maxRange, bool dealsDamage)
        {
            this.MinRange = minRange;
            this.MaxRange = maxRange;
            this.DealsDamage = dealsDamage;
        }
    }
}
