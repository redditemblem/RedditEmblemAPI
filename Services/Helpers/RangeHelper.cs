using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Models.Output.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public class RangeHelper
    {
        private readonly ItemRangeDirection[] RANGE_DIRECTIONS = new ItemRangeDirection[] { ItemRangeDirection.Northeast, ItemRangeDirection.Southeast, ItemRangeDirection.Northwest, ItemRangeDirection.Southwest };

        private IList<Unit> Units;
        private MapObj Map;

        public RangeHelper(IList<Unit> units, MapObj map)
        {
            this.Units = units;
            this.Map = map;
        }

        public void CalculateUnitRanges()
        {
            foreach (Unit unit in this.Units)
            {
                try
                {
                    //Ignore hidden units
                    if (!unit.OriginTiles.Any())
                        continue;

                    //Calculate movement range
                    int movementVal = unit.Stats["Mov"].FinalValue;
                    OverrideMovementEffect overrideMovEffect = unit.StatusConditions.Select(s => s.StatusObj.Effect).OfType<OverrideMovementEffect>().FirstOrDefault();
                    if (overrideMovEffect != null) movementVal = overrideMovEffect.MovementValue;

                    UnitRangeParameters unitParms = new UnitRangeParameters(unit);
                    RecurseUnitRange(unitParms, unit.OriginTiles.Select(o => new MovementCoordSet(movementVal, o.Coordinate)).ToList(), string.Empty, null);

                    //Calculate item ranges
                    IList<Coordinate> atkRange = new List<Coordinate>();
                    IList<Coordinate> utilRange = new List<Coordinate>();

                    IList<UnitItemRange> itemRanges = unit.Inventory.Where(i => i != null && i.CanEquip && !i.IsUsePrevented && (i.ModifiedMinRangeValue > 0 || i.ModifiedMaxRangeValue > 0))
                                                                    .Select(i => new UnitItemRange(i.ModifiedMinRangeValue, i.ModifiedMaxRangeValue, i.Item.Range.Shape, i.Item.DealsDamage, i.AllowMeleeRange))
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
                            while (itemRanges.Any(r => r.DealsDamage))
                            {
                                itemRanges.Remove(itemRanges.First(r => r.DealsDamage));
                            }
                        }

                        if (applyUtil)
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
                            foreach(ItemRangeDirection direction in RANGE_DIRECTIONS)
                            {
                                //Calculate attack range
                                ItemRangeParameters rangeParms = new ItemRangeParameters(unit, coord, itemRanges, direction);
                                RecurseItemRange(rangeParms,
                                                 coord,
                                                 rangeParms.LargestRange,
                                                 string.Empty,
                                                 ref atkRange,
                                                 ref utilRange
                                                );
                            }

                        }
                    }

                    unit.AttackRange = atkRange;
                    unit.UtilityRange = utilRange;
                }
                catch (Exception ex)
                {
                    throw new RangeCalculationException(unit, ex);
                }
            }
        }

        private void RecurseUnitRange(UnitRangeParameters parms, IList<MovementCoordSet> currCoords, string visitedCoords, Coordinate lastWarpUsed)
        {
            //Base case
            //Don't exceed the maximum range and don't go off the map
            if (currCoords.Any(c => c.RemainingMov < 0
                                 || c.Coordinate.X < 1
                                 || c.Coordinate.Y < 1
                                 || c.Coordinate.X > this.Map.TileWidth
                                 || c.Coordinate.Y > this.Map.TileHeight)
               )
                return;

            for (int i = 0; i < currCoords.Count; i++)
            {
                MovementCoordSet currCoord = currCoords[i];
                Tile tile = this.Map.GetTileByCoord(currCoords[i].Coordinate);

                //If there is a Unit occupying this tile, check for affiliation collisions
                //Check if this tile blocks units of a certain affiliation
                if ( UnitIsBlocked(parms.Unit, tile.Unit, parms.IgnoresAffiliations) ||
                    (tile.TerrainTypeObj.RestrictAffiliations.Any() && !tile.TerrainTypeObj.RestrictAffiliations.Contains(parms.Unit.AffiliationObj.Grouping))
                   ) 
                    return;

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

                if (tile.Unit != null && tile.Unit.AffiliationObj.Grouping == parms.Unit.AffiliationObj.Grouping && moveCost < 99)
                {
                    //If tile is occupied by an ally, test if they have a skill that sets the move cost
                    //Only applies if value is less than natural move cost for unit
                    OriginAllyMovementCostSetEffect allyMovCostSet = tile.Unit.SkillList.Select(s => s.Effect).OfType<OriginAllyMovementCostSetEffect>().FirstOrDefault();
                    if (allyMovCostSet != null && allyMovCostSet.MovementCost < moveCost)
                        moveCost = allyMovCostSet.MovementCost;
                }

                //Min/max value enforcement
                moveCost = Math.Max(0, moveCost);
                if (moveCost >= 99) return;

                //Don't check or subtract move cost for the starting tile
                if (visitedCoords.Length > 0)
                {
                    if (moveCost > currCoord.RemainingMov) return;
                    currCoord.RemainingMov = currCoord.RemainingMov - moveCost;
                }
            }

            //If none of the tiles fail the move cost checks above...
            //Add current anchor tile to visited coords
            visitedCoords += "_" + currCoords.First().Coordinate.ToString() + "_";

            //Document tile movement
            IList<Tile> tiles = currCoords.Select(c => this.Map.GetTileByCoord(c.Coordinate)).ToList();
            if (!tiles.Any(t => t.TerrainTypeObj.CannotStopOn))
            {
                foreach (Tile tile in tiles)
                    if (!parms.Unit.MovementRange.Contains(tile.Coordinate))
                        parms.Unit.MovementRange.Add(tile.Coordinate);
            }

            //Units may move onto obstructed tiles, but no further.
            if (tiles.Any(t => UnitIsBlocked(parms.Unit, t.ObstructingUnits, parms.IgnoresAffiliations))) return;


            //Navigate in each cardinal direction, do not repeat tiles in this path
            //Left
            //Coordinate left = new Coordinate(currCoord.X - 1, currCoord.Y);
            IList<MovementCoordSet> left = currCoords.Select(c => new MovementCoordSet(c.RemainingMov, new Coordinate(c.Coordinate.X - 1, c.Coordinate.Y))).ToList();
            if (!visitedCoords.Contains("_" + left.First().Coordinate.ToString() + "_"))
                RecurseUnitRange(parms, left, visitedCoords, lastWarpUsed);

            //Right
            //Coordinate right = new Coordinate(currCoord.X + 1, currCoord.Y);
            IList<MovementCoordSet> right = currCoords.Select(c => new MovementCoordSet(c.RemainingMov, new Coordinate(c.Coordinate.X + 1, c.Coordinate.Y))).ToList();
            if (!visitedCoords.Contains("_" + right.First().Coordinate.ToString() + "_"))
                RecurseUnitRange(parms, right, visitedCoords, lastWarpUsed);

            //Up
            //Coordinate up = new Coordinate(currCoord.X, currCoord.Y - 1);
            IList<MovementCoordSet> up = currCoords.Select(c => new MovementCoordSet(c.RemainingMov, new Coordinate(c.Coordinate.X, c.Coordinate.Y - 1))).ToList();
            if (!visitedCoords.Contains("_" + up.First().Coordinate.ToString() + "_"))
                RecurseUnitRange(parms, up, visitedCoords, lastWarpUsed);

            //Down
            //Coordinate down = new Coordinate(currCoord.X, currCoord.Y + 1);
            IList<MovementCoordSet> down = currCoords.Select(c => new MovementCoordSet(c.RemainingMov, new Coordinate(c.Coordinate.X, c.Coordinate.Y + 1))).ToList();
            if (!visitedCoords.Contains("_" + down.First().Coordinate.ToString() + "_"))
                RecurseUnitRange(parms, down, visitedCoords, lastWarpUsed);


            //If any tile is a warp entrance, calculate the remaining range from each warp exit too.
            IEnumerable<Tile> warps = tiles.Where(t => t.TerrainTypeObj.WarpType == WarpType.Entrance || t.TerrainTypeObj.WarpType == WarpType.Dual
                                              && (lastWarpUsed == null || t.Coordinate != lastWarpUsed));
            foreach (Tile warp in warps)
            {
                //Calculate warp cost
                WarpMovementCostSetEffect warpCostSet = parms.WarpCostSets.FirstOrDefault(s => warp.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
                WarpMovementCostModifierEffect warpCostMod = parms.WarpCostModifiers.FirstOrDefault(s => warp.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));

                int warpCost = warp.TerrainTypeObj.WarpCost;
                if (warpCostSet != null) warpCost = warpCostSet.Value;
                else if (warpCostMod != null) warpCost += warpCostMod.Value;

                if (warpCost < 0) warpCost = 0;

                foreach (Tile warpExit in warp.WarpGroup.Where(t => warp.Coordinate != t.Coordinate && (t.TerrainTypeObj.WarpType == WarpType.Exit || t.TerrainTypeObj.WarpType == WarpType.Dual)))
                {
                    //Calculate range from warp exit in all possible unit orientations, starting with the anchor tile
                    Coordinate currAnchor = currCoords.First().Coordinate;
                    for (int y = 0; y < parms.Unit.UnitSize; y++)
                    {
                        for (int x = 0; x < parms.Unit.UnitSize; x++)
                        {
                            RecurseUnitRange(parms,
                                             currCoords.Select(c => new MovementCoordSet(c.RemainingMov - warpCost, new Coordinate(warpExit.Coordinate.X + Math.Abs(currAnchor.X - c.Coordinate.X) - x, warpExit.Coordinate.Y + Math.Abs(currAnchor.Y - c.Coordinate.Y) - y))).ToList(),
                                             string.Empty,
                                             warpExit.Coordinate);
                        }
                    }
                }
            }
        }

        private void RecurseItemRange(ItemRangeParameters parms, Coordinate currCoord, int remainingRange, string visitedCoords, ref IList<Coordinate> atkRange, ref IList<Coordinate> utilRange)
        {
            //Base case
            //Don't exceed the maximum range and don't go off the map
            if (remainingRange < 0
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
                int horzDisplacement = Math.Abs(currCoord.X - parms.StartCoord.X);
                int verticalDisplacement = Math.Abs(currCoord.Y - parms.StartCoord.Y);
                int totalDisplacement = currCoord.DistanceFrom(parms.StartCoord);

                int pathLength = parms.LargestRange - remainingRange;

                List<UnitItemRange> validRanges = new List<UnitItemRange>();
                validRanges.AddRange(parms.Ranges.Where(r => r.Shape == ItemRangeShape.Standard
                                                                        && ((r.MinRange <= totalDisplacement //tile greater than min range away from unit
                                                                            && r.MinRange <= pathLength //tile greater than min range down the path
                                                                            && r.MaxRange >= totalDisplacement //tile less than max range from unit
                                                                            && r.MaxRange >= pathLength) //tile less than max range down path
                                                                            || (totalDisplacement == 1 && pathLength == 1 && r.AllowMeleeRange) //unit can specially allow melee range for an item
                                                                           )
                                                                        ));
                validRanges.AddRange(parms.Ranges.Where(r => r.Shape == ItemRangeShape.Square
                                                                        && (((r.MinRange <= verticalDisplacement || r.MinRange <= horzDisplacement)
                                                                              && r.MaxRange >= verticalDisplacement
                                                                              && r.MaxRange >= horzDisplacement)
                                                                            || (totalDisplacement == 1 && pathLength == 1 && r.AllowMeleeRange) //unit can specially allow melee range for an item
                                                                        )));
                validRanges.AddRange(parms.Ranges.Where(r => (r.Shape == ItemRangeShape.Cross || r.Shape == ItemRangeShape.Star)
                                                                        && (((horzDisplacement == 0 //tile vertically within range
                                                                               && r.MinRange <= verticalDisplacement
                                                                               && r.MaxRange >= verticalDisplacement)
                                                                            || (verticalDisplacement == 0 //tile horizontally within range
                                                                               && r.MinRange <= horzDisplacement
                                                                               && r.MaxRange >= horzDisplacement)
                                                                            && totalDisplacement == pathLength) //straight paths only
                                                                           || (totalDisplacement == 1 && pathLength == 1 && r.AllowMeleeRange)
                                                                           )
                                                                        ));
                validRanges.AddRange(parms.Ranges.Where(r => (r.Shape == ItemRangeShape.Saltire || r.Shape == ItemRangeShape.Star)
                                                                        && ((horzDisplacement == verticalDisplacement
                                                                               && r.MinRange <= verticalDisplacement
                                                                               && r.MaxRange >= verticalDisplacement
                                                                               && r.MinRange <= horzDisplacement
                                                                               && r.MaxRange >= horzDisplacement
                                                                               && totalDisplacement == pathLength) //straight paths only
                                                                           || (totalDisplacement == 1 && pathLength == 1 && r.AllowMeleeRange)
                                                                           )
                                                                        ));
                //Add to attacking range
                if (validRanges.Any(r => r.DealsDamage) && !atkRange.Contains(currCoord))
                    atkRange.Add(currCoord);
                //Add to util range
                else if (validRanges.Any(r => !r.DealsDamage) && !utilRange.Contains(currCoord))
                    utilRange.Add(currCoord);
            }

            //Navigate in each cardinal direction, do not repeat tiles in this path
            //Left
            if (parms.RangeDirection == ItemRangeDirection.Northwest || parms.RangeDirection == ItemRangeDirection.Southwest)
            {
                Coordinate left = new Coordinate(currCoord.X - 1, currCoord.Y);
                if (!visitedCoords.Contains("_" + left.ToString() + "_"))
                    RecurseItemRange(parms, left, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);
            }

            //Right
            if (parms.RangeDirection == ItemRangeDirection.Northeast || parms.RangeDirection == ItemRangeDirection.Southeast)
            {
                Coordinate right = new Coordinate(currCoord.X + 1, currCoord.Y);
                if (!visitedCoords.Contains("_" + right.ToString() + "_"))
                    RecurseItemRange(parms, right, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);
            }

            //Up
            if (parms.RangeDirection == ItemRangeDirection.Northwest || parms.RangeDirection == ItemRangeDirection.Northeast)
            {
                Coordinate up = new Coordinate(currCoord.X, currCoord.Y - 1);
                if (!visitedCoords.Contains("_" + up.ToString() + "_"))
                    RecurseItemRange(parms, up, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);
            }

            //Down
            if (parms.RangeDirection == ItemRangeDirection.Southwest || parms.RangeDirection == ItemRangeDirection.Southeast)
            {
                Coordinate down = new Coordinate(currCoord.X, currCoord.Y + 1);
                if (!visitedCoords.Contains("_" + down.ToString() + "_"))
                    RecurseItemRange(parms, down, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);

            }
        }

        private void ApplyWholeMapItemRange(Unit unit, bool applyAtk, bool applyUtil, ref IList<Coordinate> atkRange, ref IList<Coordinate> utilRange)
        {
            foreach (List<Tile> row in this.Map.Tiles)
            {
                foreach (Tile tile in row)
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
            if (movingUnit.AffiliationObj.Grouping == blockingUnit.AffiliationObj.Grouping)
                return false;

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
        public ItemRangeDirection RangeDirection;

        public ItemRangeParameters(Unit unit, Coordinate startCoord, IList<UnitItemRange> ranges, ItemRangeDirection direction)
        {
            this.Unit = unit;
            this.StartCoord = startCoord;
            this.Ranges = ranges;
            this.LargestRange = this.Ranges.Select(r => (r.Shape == ItemRangeShape.Square || r.Shape == ItemRangeShape.Saltire || r.Shape == ItemRangeShape.Star) ? r.MaxRange * 2 : r.MaxRange).OrderByDescending(r => r).FirstOrDefault();
            this.RangeDirection = direction;

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
        public ItemRangeShape Shape;
        public bool DealsDamage;
        public bool AllowMeleeRange;

        public UnitItemRange(int minRange, int maxRange, ItemRangeShape shape, bool dealsDamage, bool allowMeleeRange)
        {
            this.MinRange = minRange;
            this.MaxRange = maxRange;
            this.Shape = shape;
            this.DealsDamage = dealsDamage;
            this.AllowMeleeRange = allowMeleeRange;
        }
    }

    public enum ItemRangeDirection
    {
        Northwest,
        Southwest,
        Northeast,
        Southeast
    }

    public class MovementCoordSet
    {
        public int RemainingMov;
        public Coordinate Coordinate;

        public MovementCoordSet(int remainingMov, Coordinate coord)
        {
            this.RemainingMov = remainingMov;
            this.Coordinate = coord;
        }
    }
}