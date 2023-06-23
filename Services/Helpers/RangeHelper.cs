using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
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

        private List<Unit> Units;
        private MapObj Map;

        public RangeHelper(List<Unit> units, MapObj map)
        {
            this.Units = units;
            this.Map = map;
        }

        /// <summary>
        /// Calculates both movement and item ranges for all units in <c>Units</c>.
        /// </summary>
        /// <exception cref="RangeCalculationException"></exception>
        public void CalculateUnitRanges()
        {
            foreach (Unit unit in this.Units)
            {
                try
                {
                    //Ignore hidden units
                    if (!unit.Location.OriginTiles.Any())
                        continue;

                    CalculateUnitMovementRange(unit);
                    CalculateUnitItemRanges(unit);
                }
                catch (Exception ex)
                {
                    throw new RangeCalculationException(unit, ex);
                }
            }
        }

        public void CalculateTileObjectRanges()
        {
            foreach (TileObjectInstance tileObjInst in this.Map.TileObjectInstances.Values)
            {
                //If tile object does not have a configured range, skip it.
                if (tileObjInst.TileObject.Range.Minimum < 1 || tileObjInst.TileObject.Range.Maximum < 1)
                    continue;

                List<Coordinate> atkRange = new List<Coordinate>();
                List<Coordinate> utilRange = new List<Coordinate>();

                //Transpose item data into the struct we're using for recursion
                List<UnitItemRange> ranges = new List<UnitItemRange> { new UnitItemRange(tileObjInst.TileObject.Range.Minimum, tileObjInst.TileObject.Range.Maximum, ItemRangeShape.Standard, false, true, false) };

                foreach (Tile originTile in tileObjInst.OriginTiles)
                {
                    foreach (ItemRangeDirection direction in RANGE_DIRECTIONS)
                    {
                        ItemRangeParameters rangeParms = new ItemRangeParameters(originTile.Coordinate, tileObjInst.OriginTiles.Select(ot => ot.Coordinate).ToList(), ranges, direction, 0);
                        RecurseItemRange(rangeParms,
                                          originTile.Coordinate,
                                          rangeParms.LargestRange,
                                          string.Empty,
                                          ref atkRange,
                                          ref utilRange
                                        );
                    }
                }

                tileObjInst.AttackRange = atkRange;
            }
        }

        #region Movement Range Calculation

        /// <summary>
        /// Calculates the movement range for <paramref name="unit"/>.
        /// </summary>
        private void CalculateUnitMovementRange(Unit unit)
        {
            int movementVal = unit.Stats.General[this.Map.Constants.UnitMovementStatName].FinalValue;
            OverrideMovementEffect overrideMovEffect = unit.StatusConditions.SelectMany(s => s.StatusObj.Effects).OfType<OverrideMovementEffect>().FirstOrDefault();
            if (overrideMovEffect != null) movementVal = overrideMovEffect.MovementValue;

            UnitRangeParameters unitParms = new UnitRangeParameters(unit);

            if (unit.Location.UnitSize == 1) CalculateSingleTileUnitMovementRange(unitParms, movementVal);
            else CalculateMultiTileUnitMovementRange(unitParms, movementVal);
        }

        private void CalculateSingleTileUnitMovementRange(UnitRangeParameters unitParms, int movementVal)
        {
            List<CoordMapVertex> coordinateMap = this.Map.Tiles.SelectMany(c => c.Select(r => new CoordMapVertex(r.Coordinate))).ToList();
            foreach (Tile originTile in unitParms.Unit.Location.OriginTiles)
            {
                CoordMapVertex originMap = coordinateMap.Single(m => m.Coordinate == originTile.Coordinate);
                originMap.MinDistanceTo = 0;
            }

            CalculateCoordinateMapValues(unitParms, coordinateMap);
            IEnumerable<Coordinate> movementRangeCoords = coordinateMap.Where(c => c.MinDistanceTo <= movementVal && c.MinDistanceTo < 99 && !c.TraversableOnly).Select(c => c.Coordinate);

            unitParms.Unit.Ranges.Movement = unitParms.Unit.Ranges.Movement.Union(movementRangeCoords).Distinct().ToList();
        }

        private void CalculateMultiTileUnitMovementRange(UnitRangeParameters unitParms, int movementVal)
        {
            RecurseUnitRange(unitParms, unitParms.Unit.Location.OriginTiles.Select(o => new MovementCoordSet(movementVal, o.Coordinate)).ToList(), string.Empty, null);
        }

        private void RecurseUnitRange(UnitRangeParameters parms, List<MovementCoordSet> currCoords, string visitedCoords, Coordinate lastWarpUsed)
        {
            //Base case
            //Don't exceed the maximum range and don't go off the map
            if (currCoords.Any(c => c.RemainingMov < 0
                                 || c.Coordinate.X < 1
                                 || c.Coordinate.Y < 1
                                 || c.Coordinate.X > this.Map.MapWidthInTiles
                                 || c.Coordinate.Y > this.Map.MapHeightInTiles)
               )
                return;

            for (int i = 0; i < currCoords.Count; i++)
            {
                MovementCoordSet currCoord = currCoords[i];
                Tile tile = this.Map.GetTileByCoord(currCoords[i].Coordinate);

                int moveCost = CalculateTileMovementCost(parms, tile);
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
            List<Tile> tiles = currCoords.Select(c => this.Map.GetTileByCoord(c.Coordinate)).ToList();
            if (!tiles.Any(t => t.TerrainTypeObj.CannotStopOn))
            {
                foreach (Tile tile in tiles)
                    if (!parms.Unit.Ranges.Movement.Contains(tile.Coordinate))
                        parms.Unit.Ranges.Movement.Add(tile.Coordinate);
            }

            //Units may move onto obstructed tiles, but no further.
            if (tiles.Any(t => UnitIsBlocked(parms.Unit, t.UnitData.UnitsObstructingMovement, parms.IgnoresAffiliations, parms.Unit.Location.OriginTiles.Contains(t)))) return;

            //Navigate in each cardinal direction, do not repeat tiles in this path
            //Left
            //Coordinate left = new Coordinate(currCoord.X - 1, currCoord.Y);
            List<MovementCoordSet> left = currCoords.Select(c => new MovementCoordSet(c.RemainingMov, new Coordinate(this.Map.Constants.CoordinateFormat, c.Coordinate.X - 1, c.Coordinate.Y))).ToList();
            if (!visitedCoords.Contains("_" + left.First().Coordinate.ToString() + "_"))
                RecurseUnitRange(parms, left, visitedCoords, lastWarpUsed);

            //Right
            //Coordinate right = new Coordinate(currCoord.X + 1, currCoord.Y);
            List<MovementCoordSet> right = currCoords.Select(c => new MovementCoordSet(c.RemainingMov, new Coordinate(this.Map.Constants.CoordinateFormat, c.Coordinate.X + 1, c.Coordinate.Y))).ToList();
            if (!visitedCoords.Contains("_" + right.First().Coordinate.ToString() + "_"))
                RecurseUnitRange(parms, right, visitedCoords, lastWarpUsed);

            //Up
            //Coordinate up = new Coordinate(currCoord.X, currCoord.Y - 1);
            List<MovementCoordSet> up = currCoords.Select(c => new MovementCoordSet(c.RemainingMov, new Coordinate(this.Map.Constants.CoordinateFormat, c.Coordinate.X, c.Coordinate.Y - 1))).ToList();
            if (!visitedCoords.Contains("_" + up.First().Coordinate.ToString() + "_"))
                RecurseUnitRange(parms, up, visitedCoords, lastWarpUsed);

            //Down
            //Coordinate down = new Coordinate(currCoord.X, currCoord.Y + 1);
            List<MovementCoordSet> down = currCoords.Select(c => new MovementCoordSet(c.RemainingMov, new Coordinate(this.Map.Constants.CoordinateFormat, c.Coordinate.X, c.Coordinate.Y + 1))).ToList();
            if (!visitedCoords.Contains("_" + down.First().Coordinate.ToString() + "_"))
                RecurseUnitRange(parms, down, visitedCoords, lastWarpUsed);


            //If any tile is a warp entrance, calculate the remaining range from each warp exit too.
            IEnumerable<Tile> warps = tiles.Where(t => t.TerrainTypeObj.WarpType == WarpType.Entrance || t.TerrainTypeObj.WarpType == WarpType.Dual
                                              && (lastWarpUsed == null || t.Coordinate != lastWarpUsed));
            foreach (Tile warp in warps)
            {
                int warpCost = CalculateTileWarpMovementCost(parms, warp);

                foreach (Tile warpExit in warp.WarpData.WarpGroup.Where(t => warp.Coordinate != t.Coordinate && (t.TerrainTypeObj.WarpType == WarpType.Exit || t.TerrainTypeObj.WarpType == WarpType.Dual)))
                {
                    //Calculate range from warp exit in all possible unit orientations, starting with the anchor tile
                    Coordinate currAnchor = currCoords.First().Coordinate;
                    for (int y = 0; y < parms.Unit.Location.UnitSize; y++)
                    {
                        for (int x = 0; x < parms.Unit.Location.UnitSize; x++)
                        {
                            RecurseUnitRange(parms,
                                             currCoords.Select(c => new MovementCoordSet(c.RemainingMov - warpCost, new Coordinate(this.Map.Constants.CoordinateFormat, warpExit.Coordinate.X + Math.Abs(currAnchor.X - c.Coordinate.X) - x, warpExit.Coordinate.Y + Math.Abs(currAnchor.Y - c.Coordinate.Y) - y))).ToList(),
                                             string.Empty,
                                             warpExit.Coordinate);
                        }
                    }
                }
            }
        }

        private void CalculateCoordinateMapValues(UnitRangeParameters parms, List<CoordMapVertex> coordinateMap)
        {
            //Set pathing values for all tiles
            foreach (CoordMapVertex coord in coordinateMap)
            {
                Tile tile = this.Map.GetTileByCoord(coord.Coordinate);
                coord.PathCost = CalculateTileMovementCost(parms, tile);

                if (tile.TerrainTypeObj.CannotStopOn)
                    coord.TraversableOnly = true;

                //Units may move onto obstructed tiles, but no further.
                if (UnitIsBlocked(parms.Unit, tile.UnitData.UnitsObstructingMovement, parms.IgnoresAffiliations, parms.Unit.Location.OriginTiles.Contains(tile)))
                    coord.EndNode = true;
            }

            //Calculate minimum possible distance to every tile based on path values
            List<Coordinate> warpsUsed = new List<Coordinate>();

            while (coordinateMap.Any(c => !c.Visited))
            {
                CoordMapVertex currCoord = coordinateMap.Where(c => !c.Visited).MinBy(c => c.MinDistanceTo);
                currCoord.Visited = true;

                //If this node if a terminus, do not use its path value to update its neighbors
                //If this node cannot be pathed to, skip it.
                if (currCoord.EndNode || currCoord.PathCost >= 99 || currCoord.MinDistanceTo == int.MaxValue)
                    continue;

                List<CoordMapVertex> neighbors = coordinateMap.Where(c => ((currCoord.Coordinate.X == c.Coordinate.X && currCoord.Coordinate.Y == c.Coordinate.Y - 1)
                                                                          || (currCoord.Coordinate.X == c.Coordinate.X && currCoord.Coordinate.Y == c.Coordinate.Y + 1)
                                                                          || (currCoord.Coordinate.X == c.Coordinate.X - 1 && currCoord.Coordinate.Y == c.Coordinate.Y)
                                                                          || (currCoord.Coordinate.X == c.Coordinate.X + 1 && currCoord.Coordinate.Y == c.Coordinate.Y))
                                                                          && !c.Visited && c.PathCost < 99
                                                                       ).ToList();
                foreach (CoordMapVertex neighbor in neighbors)
                    neighbor.MinDistanceTo = Math.Min(neighbor.MinDistanceTo, currCoord.MinDistanceTo + neighbor.PathCost);


                //If the tile is a warp entrance, find the costs from its exit too.
                Tile tile = this.Map.GetTileByCoord(currCoord.Coordinate);
                if ((tile.TerrainTypeObj.WarpType == WarpType.Entrance || tile.TerrainTypeObj.WarpType == WarpType.Dual) && !warpsUsed.Contains(tile.Coordinate))
                {
                    warpsUsed.Add(tile.Coordinate);
                    int warpCost = CalculateTileWarpMovementCost(parms, tile);

                    bool warpUpdated = false;
                    foreach (Tile warpExit in tile.WarpData.WarpGroup.Where(t => tile.Coordinate != t.Coordinate && (t.TerrainTypeObj.WarpType == WarpType.Exit || t.TerrainTypeObj.WarpType == WarpType.Dual)))
                    {
                        CoordMapVertex currWarp = coordinateMap.First(c => c.Coordinate == warpExit.Coordinate);

                        //Ignore any exit on an untraversable tile
                        if (currWarp.PathCost >= 99)
                            continue;

                        if (currCoord.MinDistanceTo + warpCost < currWarp.MinDistanceTo)
                        {
                            currWarp.MinDistanceTo = currCoord.MinDistanceTo + warpCost;
                            warpUpdated = true;
                        }
                    }

                    //Reset visited status of all vertices
                    if (warpUpdated) coordinateMap.ForEach(c => c.Visited = false);
                }
            }
        }

        private int CalculateTileMovementCost(UnitRangeParameters parms, Tile tile)
        {
            // If there is a Unit occupying this tile, check for affiliation collisions
            // Check if this tile blocks units of a certain affiliation
            if (UnitIsBlocked(parms.Unit, tile.UnitData.Unit, parms.IgnoresAffiliations, parms.Unit.Location.OriginTiles.Contains(tile)) ||
                (tile.TerrainTypeObj.RestrictAffiliations.Any() && !tile.TerrainTypeObj.RestrictAffiliations.Contains(parms.Unit.AffiliationObj.Grouping))
               )
                return 99;

            //Test that the unit can move to this tile
            int moveCost;
            if (!tile.TerrainTypeObj.MovementCosts.TryGetValue(parms.Unit.GetUnitMovementType(), out moveCost))
                throw new UnmatchedMovementTypeException(parms.Unit.GetUnitMovementType(), tile.TerrainTypeObj.MovementCosts.Keys);

            //Apply movement cost modifiers
            TerrainTypeMovementCostSetEffect_Skill movCostSet_Skill = parms.MoveCostSets_Skills.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
            TerrainTypeMovementCostSetEffect_Status movCostSet_Status = parms.MoveCostSets_Statuses.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
            TerrainTypeMovementCostModifierEffect moveCostMod = parms.MoveCostModifiers.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
            if (movCostSet_Status != null && (movCostSet_Status.CanOverride99MoveCost || moveCost < 99))
                moveCost = movCostSet_Status.Value;
            else if (movCostSet_Skill != null && (movCostSet_Skill.CanOverride99MoveCost || moveCost < 99))
                moveCost = movCostSet_Skill.Value;
            else if (moveCostMod != null && moveCost < 99)
                moveCost += moveCostMod.Value;

            //Check if nearby units can affect the movement costs of this tile
            if (moveCost < 99 && tile.UnitData.UnitsAffectingMovementCosts.Any())
            {
                IEnumerable<IAffectMovementCost> activeEffects = tile.UnitData.UnitsAffectingMovementCosts.SelectMany(u => u.GetFullSkillsList().SelectMany(s => s.Effects).OfType<IAffectMovementCost>().Where(e => e.IsActive(u, parms.Unit)));
                if (activeEffects.Any())
                {
                    int minEffectMovCost = activeEffects.Min(e => e.GetMovementCost());
                    if (minEffectMovCost < moveCost)
                        moveCost = minEffectMovCost;
                }
            }

            //Min/max value enforcement
            moveCost = Math.Max(0, moveCost);
            moveCost = Math.Min(moveCost, 99);

            return moveCost;
        }

        private int CalculateTileWarpMovementCost(UnitRangeParameters parms, Tile warp)
        {
            WarpMovementCostSetEffect warpCostSet = parms.WarpCostSets.FirstOrDefault(s => warp.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
            WarpMovementCostModifierEffect warpCostMod = parms.WarpCostModifiers.FirstOrDefault(s => warp.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));

            int warpCost = warp.TerrainTypeObj.WarpCost;
            if (warpCostSet != null) warpCost = warpCostSet.Value;
            else if (warpCostMod != null) warpCost += warpCostMod.Value;

            warpCost = Math.Max(0, warpCost); //enforce minimum

            return warpCost;
        }

        /// <summary>
        /// Returns true if <paramref name="movingUnit"/> is blocked by any of the units in <paramref name="blockingUnits"/>.
        /// </summary>
        private bool UnitIsBlocked(Unit movingUnit, List<Unit> blockingUnits, bool ignoreAffiliations, bool isOnOriginTile)
        {
            //If unit ignores affiliations, never be blocked
            //Skip further logic
            if (ignoreAffiliations)
                return false;

            return blockingUnits.Any(u => UnitIsBlocked(movingUnit, u, ignoreAffiliations, isOnOriginTile));
        }

        /// <summary>
        /// Returns true if <paramref name="movingUnit"/> is blocked by <paramref name="blockingUnit"/>.
        /// </summary>
        private bool UnitIsBlocked(Unit movingUnit, Unit blockingUnit, bool ignoreAffiliations, bool isOnOriginTile)
        {
            //If unit ignores affiliations, never be blocked
            if (ignoreAffiliations)
                return false;

            //Check if both units exist
            if (movingUnit == null || blockingUnit == null)
                return false;

            //If the moving unit is on its origin tile, it is not impacted by blocking units
            if (isOnOriginTile)
                return false;

            //Check if units are the same
            if (movingUnit.Name == blockingUnit.Name)
                return false;

            //Check if the blocking unit is inflicted with a relevant status condition
            if (blockingUnit.StatusConditions.SelectMany(sc => sc.StatusObj.Effects).OfType<DoesNotBlockEnemyAffiliationsEffect>().Any())
                return false;

            //Finally, check if units are not in the same affiliation grouping
            return movingUnit.AffiliationObj.Grouping != blockingUnit.AffiliationObj.Grouping;
        }

        #endregion Movement Range Calculation

        #region Item Range Calculation

        /// <summary>
        /// Calculates all item ranges for <paramref name="unit"/>.
        /// </summary>
        private void CalculateUnitItemRanges(Unit unit)
        {
            List<Coordinate> atkRange = new List<Coordinate>();
            List<Coordinate> utilRange = new List<Coordinate>();

            //Transpose item data into the struct we're using for recursion
            List<UnitItemRange> itemRanges = SelectInventoryItemsIntoRangeStruct(unit.Inventory.Items);

            //If unit is engaged with an emblem, include its items in the range as well
            if (unit.Emblem != null && unit.Emblem.IsEngaged)
                itemRanges = itemRanges.Union(SelectInventoryItemsIntoRangeStruct(unit.Emblem.EngageWeapons)).ToList();

            //Check for special case ranges
            ApplyWholeMapItemRanges(unit, itemRanges, ref atkRange, ref utilRange);
            ApplyNoUnitMovementItemRanges(unit, itemRanges, ref atkRange, ref utilRange);

            //Calculate any remainging ranges normally
            if (itemRanges.Any())
            {
                foreach (Coordinate coord in unit.Ranges.Movement)
                {
                    foreach (ItemRangeDirection direction in RANGE_DIRECTIONS)
                    {
                        ItemRangeParameters rangeParms = new ItemRangeParameters(coord, unit.Ranges.Movement, itemRanges, direction, unit.AffiliationObj.Grouping);
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

            unit.Ranges.Attack = atkRange;
            unit.Ranges.Utility = utilRange;
        }

        private List<UnitItemRange> SelectInventoryItemsIntoRangeStruct(List<UnitInventoryItem> items)
        {
            return items.Where(i => i.CanEquip && !i.IsUsePrevented && !i.MaxRangeExceedsCalculationLimit && (i.MinRange.FinalValue > 0 || i.MaxRange.FinalValue > 0))
                        .Select(i => new UnitItemRange(i.MinRange.FinalValue, i.MaxRange.FinalValue, i.Item.Range.Shape, i.Item.Range.CanOnlyUseBeforeMovement, i.Item.DealsDamage, i.AllowMeleeRange))
                        .ToList();
        }

        private void RecurseItemRange(ItemRangeParameters parms, Coordinate currCoord, int remainingRange, string visitedCoords, ref List<Coordinate> atkRange, ref List<Coordinate> utilRange)
        {
            //Base case
            //Don't exceed the maximum range and don't go off the map
            if (remainingRange < 0
                || currCoord.X < 1
                || currCoord.Y < 1
                || currCoord.X > this.Map.MapWidthInTiles
                || currCoord.Y > this.Map.MapHeightInTiles
               )
                return;

            Tile tile = this.Map.GetTileByCoord(currCoord);

            //Check if ranges can pass through this tile
            if (tile.TerrainTypeObj.BlocksItems)
                return;

            //Check for items that can reach this tile
            if (!parms.IgnoreTiles.Contains(currCoord))
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

            //Don't make these checks on the starting tile
            if (!string.IsNullOrEmpty(visitedCoords))
            {
                //Check if range can continue past this point
                if (tile.UnitData.UnitsObstructingItems.Any(u => u.AffiliationObj.Grouping != parms.AffiliationGrouping))
                    return;
            }

            visitedCoords += "_" + currCoord.ToString() + "_";

            //Navigate in each cardinal direction, do not repeat tiles in this path
            //Left
            if (parms.RangeDirection == ItemRangeDirection.Northwest || parms.RangeDirection == ItemRangeDirection.Southwest)
            {
                Coordinate left = new Coordinate(this.Map.Constants.CoordinateFormat, currCoord.X - 1, currCoord.Y);
                if (!visitedCoords.Contains("_" + left.ToString() + "_"))
                    RecurseItemRange(parms, left, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);
            }

            //Right
            if (parms.RangeDirection == ItemRangeDirection.Northeast || parms.RangeDirection == ItemRangeDirection.Southeast)
            {
                Coordinate right = new Coordinate(this.Map.Constants.CoordinateFormat, currCoord.X + 1, currCoord.Y);
                if (!visitedCoords.Contains("_" + right.ToString() + "_"))
                    RecurseItemRange(parms, right, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);
            }

            //Up
            if (parms.RangeDirection == ItemRangeDirection.Northwest || parms.RangeDirection == ItemRangeDirection.Northeast)
            {
                Coordinate up = new Coordinate(this.Map.Constants.CoordinateFormat, currCoord.X, currCoord.Y - 1);
                if (!visitedCoords.Contains("_" + up.ToString() + "_"))
                    RecurseItemRange(parms, up, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);
            }

            //Down
            if (parms.RangeDirection == ItemRangeDirection.Southwest || parms.RangeDirection == ItemRangeDirection.Southeast)
            {
                Coordinate down = new Coordinate(this.Map.Constants.CoordinateFormat, currCoord.X, currCoord.Y + 1);
                if (!visitedCoords.Contains("_" + down.ToString() + "_"))
                    RecurseItemRange(parms, down, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);

            }
        }

        private void ApplyWholeMapItemRanges(Unit unit, List<UnitItemRange> itemRanges, ref List<Coordinate> atkRange, ref List<Coordinate> utilRange)
        {
            //Only continue if we have at least one item with a max range of 99.
            if (!itemRanges.Any(r => r.MaxRange >= 99))
                return;

            bool applyAtk = itemRanges.Any(r => r.DealsDamage && r.MaxRange >= 99);
            bool applyUtil = itemRanges.Any(r => !r.DealsDamage && r.MaxRange >= 99);

            foreach (List<Tile> row in this.Map.Tiles)
            {
                foreach (Tile tile in row)
                {
                    //Only exclude tiles that the unit can move to or block items
                    if (!unit.Ranges.Movement.Contains(tile.Coordinate) && !tile.TerrainTypeObj.BlocksItems)
                    {
                        if (applyAtk) atkRange.Add(tile.Coordinate);
                        if (applyUtil) utilRange.Add(tile.Coordinate);
                    }
                }
            }

            //Remove all relevant ranges from list
            //Since we cover the whole map we don't need to address these individually later
            if (applyAtk)
                itemRanges.RemoveAll(r => r.DealsDamage);

            if (applyUtil)
                itemRanges.RemoveAll(r => !r.DealsDamage);
        }

        private void ApplyNoUnitMovementItemRanges(Unit unit, List<UnitItemRange> itemRanges, ref List<Coordinate> atkRange, ref List<Coordinate> utilRange)
        {
            //Only continue if we have at least one item that can only be used before movement
            if (!itemRanges.Any(r => r.CanOnlyUseBeforeMovement))
                return;

            List<UnitItemRange> noMovementItemRanges = itemRanges.Where(r => r.CanOnlyUseBeforeMovement).ToList();

            //Only calculate the item ranges for these items from the unit's origin tiles, not their whole movement range
            foreach (Coordinate coord in unit.Location.OriginTiles.Select(t => t.Coordinate))
            {
                foreach (ItemRangeDirection direction in RANGE_DIRECTIONS)
                {
                    ItemRangeParameters rangeParms = new ItemRangeParameters(coord, unit.Ranges.Movement, noMovementItemRanges, direction, unit.AffiliationObj.Grouping);
                    RecurseItemRange(rangeParms,
                                     coord,
                                     rangeParms.LargestRange,
                                     string.Empty,
                                     ref atkRange,
                                     ref utilRange
                                    );
                }
            }

            //Remove all items from the list so they aren't processed again
            itemRanges.RemoveAll(r => r.CanOnlyUseBeforeMovement);
        }

        #endregion Item Range Calculation

    }

    public struct UnitRangeParameters
    {
        public Unit Unit;

        #region Skill Effects

        public bool IgnoresAffiliations { get; set; }

        public IEnumerable<TerrainTypeMovementCostModifierEffect> MoveCostModifiers { get; set; }
        public IEnumerable<TerrainTypeMovementCostSetEffect_Skill> MoveCostSets_Skills { get; set; }
        public IEnumerable<TerrainTypeMovementCostSetEffect_Status> MoveCostSets_Statuses { get; set; }
        public IEnumerable<WarpMovementCostModifierEffect> WarpCostModifiers { get; set; }
        public IEnumerable<WarpMovementCostSetEffect> WarpCostSets { get; set; }

        #endregion

        public UnitRangeParameters(Unit unit)
        {
            this.Unit = unit;
            IEnumerable<Skill> skillList = unit.GetFullSkillsList();

            this.IgnoresAffiliations = skillList.SelectMany(s => s.Effects).OfType<IIgnoreUnitAffiliations>().Any(e => e.IsActive(unit));
            this.MoveCostModifiers = skillList.SelectMany(s => s.Effects).OfType<TerrainTypeMovementCostModifierEffect>();
            this.MoveCostSets_Skills = skillList.SelectMany(s => s.Effects).OfType<TerrainTypeMovementCostSetEffect_Skill>();
            this.MoveCostSets_Statuses = unit.StatusConditions.SelectMany(s => s.StatusObj.Effects).OfType<TerrainTypeMovementCostSetEffect_Status>();
            this.WarpCostModifiers = skillList.SelectMany(s => s.Effects).OfType<WarpMovementCostModifierEffect>();
            this.WarpCostSets = skillList.SelectMany(s => s.Effects).OfType<WarpMovementCostSetEffect>();
        }
    }

    public struct ItemRangeParameters
    {
        public Coordinate StartCoord;
        public List<Coordinate> IgnoreTiles;
        public List<UnitItemRange> Ranges;
        public int LargestRange;
        public ItemRangeDirection RangeDirection;
        public int AffiliationGrouping;

        public ItemRangeParameters(Coordinate startCoord, List<Coordinate> ignoreTiles, List<UnitItemRange> ranges, ItemRangeDirection direction, int affiliationGrouping)
        {
            this.IgnoreTiles = ignoreTiles;
            this.StartCoord = startCoord;
            this.Ranges = ranges;
            this.LargestRange = this.Ranges.Select(r => (r.Shape == ItemRangeShape.Square || r.Shape == ItemRangeShape.Saltire || r.Shape == ItemRangeShape.Star) ? r.MaxRange * 2 : r.MaxRange).OrderByDescending(r => r).FirstOrDefault();
            this.RangeDirection = direction;
            this.AffiliationGrouping = affiliationGrouping;

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
        public bool CanOnlyUseBeforeMovement;
        public bool DealsDamage;
        public bool AllowMeleeRange;

        public UnitItemRange(decimal minRange, decimal maxRange, ItemRangeShape shape, bool canOnlyBeUsedBeforeMovement, bool dealsDamage, bool allowMeleeRange)
        {
            this.MinRange = (int)decimal.Floor(minRange);
            this.MaxRange = (int)decimal.Floor(maxRange);
            this.Shape = shape;
            this.CanOnlyUseBeforeMovement = canOnlyBeUsedBeforeMovement;
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

    public class CoordMapVertex
    {
        public Coordinate Coordinate;
        public int MinDistanceTo;
        public int PathCost;
        public bool Visited;
        public bool TraversableOnly;
        public bool EndNode;

        public CoordMapVertex(Coordinate coord)
        {
            this.Coordinate = coord;
            this.MinDistanceTo = int.MaxValue;
            this.PathCost = int.MaxValue;
            this.Visited = false;
            this.TraversableOnly = false;
            this.EndNode = false;
        }
    }
}