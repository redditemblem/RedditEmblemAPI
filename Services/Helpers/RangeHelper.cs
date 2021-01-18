using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public class RangeHelper
    {
        private IList<Unit> Units;
        private MapObj Map;

        public RangeHelper(IList<Unit> units, MapObj map)
        {
            this.Units = units;
            this.Map = map;
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
                    RecurseUnitRange(unitParms, unit.OriginTile.Coordinate, unit.Stats["Mov"].FinalValue, string.Empty, null);

                    //Calculate item ranges
                    IList<Coordinate> atkRange = new List<Coordinate>();
                    IList<Coordinate> utilRange = new List<Coordinate>();

                    IList<UnitItemRange> itemRanges = unit.Inventory.Where(i => i != null && i.CanEquip && i.Item.UtilizedStats.Any() && (i.ModifiedMinRangeValue > 0 || i.ModifiedMaxRangeValue > 0))
                                                                    .Select(i => new UnitItemRange(i.ModifiedMinRangeValue, i.ModifiedMaxRangeValue, i.Item.DealsDamage, i.AllowMeleeRange))
                                                                    .ToList();
                    //Check for whole map ranges
                    if (itemRanges.Any(r => r.MaxRange >= 99))
                    {
                        bool applyAtk = itemRanges.Any(r => r.DealsDamage && r.MaxRange >= 99);
                        bool applyUtil = itemRanges.Any(r => !r.DealsDamage && r.MaxRange >= 99);

                        ApplyWholeMapItemRange(unit, applyAtk, applyUtil, ref atkRange, ref utilRange);

                        //Remove all relevant ranges from list
                        //Since we cover the whole map we don't need to address these individually later
                        if (applyAtk)
                        {
                            while(itemRanges.Any(r => r.DealsDamage))
                            {
                                itemRanges.Remove(itemRanges.First(r => r.DealsDamage));
                            }
                        }

                        if(applyUtil)
                        {
                            while (itemRanges.Any(r => !r.DealsDamage))
                            {
                                itemRanges.Remove(itemRanges.First(r => !r.DealsDamage));
                            }
                        }
                    }

                    //Check for regular ranges
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

        private void RecurseUnitRange(UnitRangeParameters parms, Coordinate currCoord, int remainingMov, string visitedCoords, Coordinate lastWarpUsed)
        {
            //Base case
            //Don't exceed the maximum range and don't go off the map
            if (   remainingMov < 0
                || currCoord.X < 1
                || currCoord.Y < 1
                || currCoord.X > this.Map.TileWidth
                || currCoord.Y > this.Map.TileHeight
               )
                return;

            Tile tile = this.Map.GetTileByCoord(currCoord);

            //If there is a Unit occupying this tile, check for affiliation collisions
            if (UnitIsBlocked(parms.Unit, tile.Unit, parms.IgnoresAffiliations)) return;

            //Test that the unit can move to this tile
            int moveCost;
            if (!tile.TerrainTypeObj.MovementCosts.TryGetValue(parms.Unit.GetUnitMovementType(), out moveCost))
                throw new UnmatchedMovementTypeException(parms.Unit.GetUnitMovementType(), tile.TerrainTypeObj.MovementCosts.Keys.ToList());

            //Apply movement cost modifiers
            TerrainTypeMovementCostSetEffect movCostSet = parms.MoveCostSets.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
            TerrainTypeMovementCostModifierEffect moveCostMod = parms.MoveCostModifiers.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
            if (movCostSet != null && (movCostSet.CanOverride99MoveCost || moveCost < 99))
                moveCost = movCostSet.Value;
            else if (moveCostMod != null && moveCost < 99)
                moveCost += moveCostMod.Value;

            if (moveCost < 0) moveCost = 0;
            if (moveCost >= 99) return;

            //Don't check or subtract move cost for the starting tile
            if (visitedCoords.Length > 0)
            {
                if (moveCost > remainingMov) return;
                remainingMov = remainingMov - moveCost;
            }

            //Make sure multi-tile units fit here
            if (parms.Unit.UnitSize > 1 && !UnitCanAccessAllIntersectedTiles(parms, tile))
                return;

            //Document tile movement
            visitedCoords += "_" + currCoord.ToString() + "_";
            if (!parms.Unit.MovementRange.Contains(currCoord))
                parms.Unit.MovementRange.Add(currCoord);

            //Units may move onto obstructed tiles, but no further.
            if (UnitIsBlocked(parms.Unit, tile.ObstructingUnits, parms.IgnoresAffiliations)) return;

            //Navigate in each cardinal direction, do not repeat tiles in this path
            //Left
            Coordinate left = new Coordinate(currCoord.X - 1, currCoord.Y);
            if (!visitedCoords.Contains("_" + left.ToString() + "_"))
                RecurseUnitRange(parms, left, remainingMov, visitedCoords, lastWarpUsed);

            //Right
            Coordinate right = new Coordinate(currCoord.X + 1, currCoord.Y);
            if (!visitedCoords.Contains("_" + right.ToString() + "_"))
                RecurseUnitRange(parms, right, remainingMov, visitedCoords, lastWarpUsed);

            //Up
            Coordinate up = new Coordinate(currCoord.X, currCoord.Y - 1);
            if (!visitedCoords.Contains("_" + up.ToString() + "_"))
                RecurseUnitRange(parms, up, remainingMov, visitedCoords, lastWarpUsed);

            //Down
            Coordinate down = new Coordinate(currCoord.X, currCoord.Y + 1);
            if (!visitedCoords.Contains("_" + down.ToString() + "_"))
                RecurseUnitRange(parms, down, remainingMov, visitedCoords, lastWarpUsed);

            //If this tile is a warp entrance, calculate the remaining range from each warp exit too.
            if (    (tile.TerrainTypeObj.WarpType == WarpType.Entrance || tile.TerrainTypeObj.WarpType == WarpType.Dual)
                 && (lastWarpUsed == null || tile.Coordinate != lastWarpUsed)
               )
            {
                //Calculate warp cost
                WarpMovementCostSetEffect warpCostSet = parms.WarpCostSets.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
                WarpMovementCostModifierEffect warpCostMod = parms.WarpCostModifiers.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));

                int warpCost = tile.TerrainTypeObj.WarpCost;
                if (warpCostSet != null) warpCost = warpCostSet.Value;
                else if (warpCostMod != null) warpCost += warpCostMod.Value;

                if (warpCost < 0) warpCost = 0;

                foreach (Tile warpExit in tile.WarpGroup.Where(t => tile.Coordinate != t.Coordinate && (t.TerrainTypeObj.WarpType == WarpType.Exit || t.TerrainTypeObj.WarpType == WarpType.Dual)))
                {
                    RecurseUnitRange(parms, warpExit.Coordinate, remainingMov - warpCost, string.Empty, warpExit.Coordinate);
                }
            }
        }

        private void RecurseItemRange(ItemRangeParameters parms, Coordinate currCoord, int remainingRange, string visitedCoords, ref IList<Coordinate> atkRange, ref IList<Coordinate> utilRange)
        {
            //Base case
            //Don't exceed the maximum range and don't go off the map
            if (   remainingRange < 0 
                || currCoord.X < 1
                || currCoord.Y < 1
                || currCoord.X > this.Map.TileWidth
                || currCoord.Y > this.Map.TileHeight
               )
                return;

            Tile tile = this.Map.GetTileByCoord(currCoord);

            //Check if ranges can pass through this tile
            if (tile.TerrainTypeObj.BlocksItems)
                return;

            visitedCoords += "_" + currCoord.ToString() + "_";

            //Check for items that can reach this tile
            if (!parms.Unit.MovementRange.Contains(currCoord))
            {
                int displacement = currCoord.DistanceFrom(parms.StartCoord);
                int pathLength = parms.LargestRange - remainingRange;

                IList<UnitItemRange> validRanges = parms.Ranges.Where(r => (r.MinRange <= displacement //tile greater than min range away from unit
                                                                         && r.MinRange <= pathLength //tile greater than min range down the path
                                                                         && r.MaxRange >= displacement //tile less than max range from unit
                                                                         && r.MaxRange >= pathLength) //tile less than max range down path
                                                                        || (displacement == 1
                                                                         && pathLength == 1
                                                                         && r.AllowMeleeRange) //unit can specially allow melee range for an item
                                                                     ).ToList();
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

        private void ApplyWholeMapItemRange(Unit unit, bool applyAtk, bool applyUtil, ref IList<Coordinate> atkRange, ref IList<Coordinate> utilRange)
        {
            foreach(List<Tile> row in this.Map.Tiles)
            {
                foreach(Tile tile in row)
                {
                    //Only exclude tiles that the unit can move to or block items
                    if (!unit.MovementRange.Contains(tile.Coordinate) && !tile.TerrainTypeObj.BlocksItems)
                    {
                        if (applyAtk) atkRange.Add(tile.Coordinate);
                        if (applyUtil) utilRange.Add(tile.Coordinate);
                    }
                }
            }
        }

        private bool UnitIsBlocked(Unit movingUnit, IList<Unit> blockingUnits, bool ignoreAffiliations)
        {
            //If unit ignores affiliations, never be blocked
            //Skip further logic
            if (ignoreAffiliations)
                return false;

            return blockingUnits.Any(u => UnitIsBlocked(movingUnit, u, ignoreAffiliations));
        }

        private bool UnitIsBlocked(Unit movingUnit, Unit blockingUnit, bool ignoreAffiliations)
        {
            //If unit ignores affiliations, never be blocked
            if (ignoreAffiliations)
                return false;

            //Check if both units exist
            if (movingUnit == null || blockingUnit == null)
                return false;

            //Check if units are the same
            if (movingUnit.Name == blockingUnit.Name)
                return false;

            //Check if units are in the same affiliation grouping
            if(movingUnit.AffiliationObj.Grouping == blockingUnit.AffiliationObj.Grouping)
                return false;

            return true;
        }

        private bool UnitCanAccessAllIntersectedTiles(UnitRangeParameters unitParms, Tile currentOriginTile)
        {
            int anchorOffset = (int)Math.Ceiling(unitParms.Unit.UnitSize / 2.0m) - 1;

            //Make sure the relative anchor doesn't fall off the map
            if( currentOriginTile.Coordinate.X - anchorOffset < 1
             || currentOriginTile.Coordinate.Y - anchorOffset < 1
             || currentOriginTile.Coordinate.X - anchorOffset > this.Map.TileWidth
             || currentOriginTile.Coordinate.Y - anchorOffset > this.Map.TileHeight
              )
                return false;

            Tile relativeAnchorTile = this.Map.GetTileByCoord(currentOriginTile.Coordinate.X - anchorOffset, currentOriginTile.Coordinate.Y - anchorOffset);

            for (int y = 0; y < unitParms.Unit.UnitSize; y++)
            {
                for (int x = 0; x < unitParms.Unit.UnitSize; x++)
                {
                    //We don't need to recheck the current origin
                    if (currentOriginTile.Coordinate.X == relativeAnchorTile.Coordinate.X + x && currentOriginTile.Coordinate.Y == relativeAnchorTile.Coordinate.Y + y)
                        continue;

                    Tile tile = this.Map.GetTileByCoord(relativeAnchorTile.Coordinate.X + x, relativeAnchorTile.Coordinate.Y + y);

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
    }

    public struct UnitRangeParameters
    {
        public Unit Unit;

        #region Skill Effects

        public bool IgnoresAffiliations { get; set; }

        public IList<TerrainTypeMovementCostModifierEffect> MoveCostModifiers { get; set; }
        public IList<TerrainTypeMovementCostSetEffect> MoveCostSets { get; set; }

        public IList<WarpMovementCostModifierEffect> WarpCostModifiers { get; set; }
        public IList<WarpMovementCostSetEffect> WarpCostSets { get; set; }

        #endregion

        public UnitRangeParameters(Unit unit)
        {
            this.Unit = unit;

            this.IgnoresAffiliations = unit.SkillList.Select(s => s.Effect).OfType<IIgnoreUnitAffiliations>().Any(e => e.IsActive(unit));
            this.MoveCostModifiers = unit.SkillList.Select(s => s.Effect).OfType<TerrainTypeMovementCostModifierEffect>().ToList();
            this.MoveCostSets = unit.SkillList.Select(s => s.Effect).OfType<TerrainTypeMovementCostSetEffect>().ToList();
            this.WarpCostModifiers = unit.SkillList.Select(s => s.Effect).OfType<WarpMovementCostModifierEffect>().ToList();
            this.WarpCostSets = unit.SkillList.Select(s => s.Effect).OfType<WarpMovementCostSetEffect>().ToList();
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

            //Safeguard just in case. We shouldn't ever get a 99 range here.
            if (this.LargestRange >= 99)
                throw new Exception("Safeguard reached. Attempting to calculate a 99 range when none should ever exist at this point.");
        }
    }

    /// <summary>
    /// Represents the range parameters of a specific <c>UnitInventoryItem</c>.
    /// </summary>
    public struct UnitItemRange
    {
        public int MinRange;
        public int MaxRange;
        public bool DealsDamage;
        public bool AllowMeleeRange;

        public UnitItemRange(int minRange, int maxRange, bool dealsDamage, bool allowMeleeRange)
        {
            this.MinRange = minRange;
            this.MaxRange = maxRange;
            this.DealsDamage = dealsDamage;
            this.AllowMeleeRange = allowMeleeRange;
        }
    }
}
