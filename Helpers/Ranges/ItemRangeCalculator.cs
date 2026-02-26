using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Helpers.Ranges
{
    public class ItemRangeCalculator
    {
        #region Attributes

        private readonly OrdinalDirection[] RANGE_DIRECTIONS = new OrdinalDirection[] { OrdinalDirection.Northeast, OrdinalDirection.Southeast, OrdinalDirection.Northwest, OrdinalDirection.Southwest };

        private IEnumerable<IUnit> Units;
        private IMapObj Map;

        public ItemRangeCalculator(IMapObj map, IEnumerable<IUnit> units)
        {
            Map = map;
            Units = units;
        }

        #endregion Attributes

        //public void CalculateTileObjectRanges()
        //{
        //    foreach (TileObjectInstance tileObjInst in Map.TileObjectInstances.Values)
        //    {
        //        //If tile object does not have a configured range, skip it.
        //        if (tileObjInst.TileObject.Range.Minimum < 1 || tileObjInst.TileObject.Range.Maximum < 1)
        //            continue;

        //        List<ICoordinate> atkRange = new List<ICoordinate>();
        //        List<ICoordinate> utilRange = new List<ICoordinate>();

        //        //Transpose item data into the struct we're using for recursion
        //        List<UnitItemRange> ranges = new List<UnitItemRange> { new UnitItemRange(tileObjInst.TileObject.Range.Minimum, tileObjInst.TileObject.Range.Maximum, ItemRangeShape.Standard, false, true, false) };

        //        foreach (ITile originTile in tileObjInst.OriginTiles)
        //        {
        //            foreach (CompassDirection direction in RANGE_DIRECTIONS)
        //            {
        //                ItemRangeParameters rangeParms = new ItemRangeParameters(originTile.Coordinate, tileObjInst.OriginTiles.Select(ot => ot.Coordinate).ToList(), ranges, direction, 0);
        //                RecurseItemRange(rangeParms,
        //                                  originTile.Coordinate,
        //                                  rangeParms.LargestRange,
        //                                  string.Empty,
        //                                  ref atkRange,
        //                                  ref utilRange
        //                                );
        //            }
        //        }

        //        tileObjInst.AttackRange = atkRange;
        //    }
        //}

        ///// <summary>
        ///// Calculates all item ranges for <paramref name="unit"/>.
        ///// </summary>
        //private void CalculateUnitItemRanges(IUnit unit)
        //{
        //    List<ICoordinate> atkRange = new List<ICoordinate>();
        //    List<ICoordinate> utilRange = new List<ICoordinate>();

        //    //Transpose item data into the struct we're using for recursion
        //    List<UnitItemRange> itemRanges = SelectInventoryItemsIntoRangeStruct(unit.Inventory.GetAllItems());

        //    //If unit is engaged with an emblem, include its items in the range as well
        //    if (unit.Emblem != null && unit.Emblem.IsEngaged)
        //        itemRanges = itemRanges.Union(SelectInventoryItemsIntoRangeStruct(unit.Emblem.EngageWeapons)).ToList();

        //    //Check for special case ranges
        //    ApplyWholeMapItemRanges(unit, itemRanges, ref atkRange, ref utilRange);
        //    ApplyNoUnitMovementItemRanges(unit, itemRanges, ref atkRange, ref utilRange);

        //    //Calculate any remainging ranges normally
        //    if (itemRanges.Any())
        //    {
        //        foreach (ICoordinate coord in unit.Ranges.Movement)
        //        {
        //            foreach (CompassDirection direction in RANGE_DIRECTIONS)
        //            {
        //                ItemRangeParameters rangeParms = new ItemRangeParameters(coord, unit.Ranges.Movement, itemRanges, direction, unit.Affiliation.Grouping);
        //                RecurseItemRange(rangeParms,
        //                                 coord,
        //                                 rangeParms.LargestRange,
        //                                 string.Empty,
        //                                 ref atkRange,
        //                                 ref utilRange
        //                                );
        //            }

        //        }
        //    }

        //    unit.Ranges.Attack = atkRange;
        //    unit.Ranges.Utility = utilRange;
        //}

        ///// <summary>
        ///// Calculates all item ranges for <paramref name="unit"/>.
        ///// </summary>
        //private void CalculateUnitItemRanges(IUnit unit)
        //{
        //    List<ICoordinate> atkRange = new List<ICoordinate>();
        //    List<ICoordinate> utilRange = new List<ICoordinate>();

        //    //Transpose item data into the struct we're using for recursion
        //    List<UnitItemRange> itemRanges = SelectInventoryItemsIntoRangeStruct(unit.Inventory.GetAllItems());

        //    //If unit is engaged with an emblem, include its items in the range as well
        //    if (unit.Emblem != null && unit.Emblem.IsEngaged)
        //        itemRanges = itemRanges.Union(SelectInventoryItemsIntoRangeStruct(unit.Emblem.EngageWeapons)).ToList();

        //    //Check for special case ranges
        //    ApplyWholeMapItemRanges(unit, itemRanges, ref atkRange, ref utilRange);
        //    ApplyNoUnitMovementItemRanges(unit, itemRanges, ref atkRange, ref utilRange);

        //    //Calculate any remainging ranges normally
        //    if (itemRanges.Any())
        //    {
        //        foreach (ICoordinate coord in unit.Ranges.Movement)
        //        {
        //            foreach (CompassDirection direction in RANGE_DIRECTIONS)
        //            {
        //                ItemRangeParameters rangeParms = new ItemRangeParameters(coord, unit.Ranges.Movement, itemRanges, direction, unit.Affiliation.Grouping);
        //                RecurseItemRange(rangeParms,
        //                                 coord,
        //                                 rangeParms.LargestRange,
        //                                 string.Empty,
        //                                 ref atkRange,
        //                                 ref utilRange
        //                                );
        //            }

        //        }
        //    }

        //    unit.Ranges.Attack = atkRange;
        //    unit.Ranges.Utility = utilRange;
        //}

        //private List<UnitItemRange> SelectInventoryItemsIntoRangeStruct(IList<IUnitInventoryItem> items)
        //{
        //    return items.Where(i => i.CanEquip && !i.IsUsePrevented && !i.MaxRangeExceedsCalculationLimit && (i.MinRange.FinalValue > 0 || i.MaxRange.FinalValue > 0))
        //                .Select(i => new UnitItemRange(i.MinRange.FinalValue, i.MaxRange.FinalValue, i.Item.Range.Shape, i.Item.Range.CanOnlyUseBeforeMovement, i.Item.DealsDamage, i.AllowMeleeRange))
        //                .ToList();
        //}

        //private void RecurseItemRange(ItemRangeParameters parms, ICoordinate currCoord, int remainingRange, string visitedCoords, ref List<ICoordinate> atkRange, ref List<ICoordinate> utilRange)
        //{
        //    //Base case
        //    //Don't exceed the maximum range and don't go off the map
        //    if (remainingRange < 0
        //        || currCoord.X < 1
        //        || currCoord.Y < 1
        //        || currCoord.X > Map.MapWidthInTiles
        //        || currCoord.Y > Map.MapHeightInTiles
        //       )
        //        return;

        //    ITile tile = Map.GetTileByCoord(currCoord);

        //    //Check if ranges can pass through this tile
        //    if (tile.TerrainType.BlocksItems)
        //        return;

        //    //Check for items that can reach this tile
        //    if (!parms.IgnoreTiles.Contains(currCoord))
        //    {
        //        int horzDisplacement = Math.Abs(currCoord.X - parms.StartCoord.X);
        //        int verticalDisplacement = Math.Abs(currCoord.Y - parms.StartCoord.Y);
        //        int totalDisplacement = currCoord.DistanceFrom(parms.StartCoord);

        //        int pathLength = parms.LargestRange - remainingRange;

        //        List<UnitItemRange> validRanges = new List<UnitItemRange>();
        //        validRanges.AddRange(parms.Ranges.Where(r => r.Shape == ItemRangeShape.Standard
        //                                                                && (r.MinRange <= totalDisplacement //tile greater than min range away from unit
        //                                                                    && r.MinRange <= pathLength //tile greater than min range down the path
        //                                                                    && r.MaxRange >= totalDisplacement //tile less than max range from unit
        //                                                                    && r.MaxRange >= pathLength //tile less than max range down path
        //                                                                    || totalDisplacement == 1 && pathLength == 1 && r.AllowMeleeRange //unit can specially allow melee range for an item
        //                                                                   )
        //                                                                ));
        //        validRanges.AddRange(parms.Ranges.Where(r => r.Shape == ItemRangeShape.Square
        //                                                                && ((r.MinRange <= verticalDisplacement || r.MinRange <= horzDisplacement)
        //                                                                      && r.MaxRange >= verticalDisplacement
        //                                                                      && r.MaxRange >= horzDisplacement
        //                                                                    || totalDisplacement == 1 && pathLength == 1 && r.AllowMeleeRange //unit can specially allow melee range for an item
        //                                                                )));
        //        validRanges.AddRange(parms.Ranges.Where(r => (r.Shape == ItemRangeShape.Cross || r.Shape == ItemRangeShape.Star)
        //                                                                && (horzDisplacement == 0 //tile vertically within range
        //                                                                       && r.MinRange <= verticalDisplacement
        //                                                                       && r.MaxRange >= verticalDisplacement
        //                                                                    || verticalDisplacement == 0 //tile horizontally within range
        //                                                                       && r.MinRange <= horzDisplacement
        //                                                                       && r.MaxRange >= horzDisplacement
        //                                                                    && totalDisplacement == pathLength //straight paths only
        //                                                                   || totalDisplacement == 1 && pathLength == 1 && r.AllowMeleeRange
        //                                                                   )
        //                                                                ));
        //        validRanges.AddRange(parms.Ranges.Where(r => (r.Shape == ItemRangeShape.Saltire || r.Shape == ItemRangeShape.Star)
        //                                                                && (horzDisplacement == verticalDisplacement
        //                                                                       && r.MinRange <= verticalDisplacement
        //                                                                       && r.MaxRange >= verticalDisplacement
        //                                                                       && r.MinRange <= horzDisplacement
        //                                                                       && r.MaxRange >= horzDisplacement
        //                                                                       && totalDisplacement == pathLength //straight paths only
        //                                                                   || totalDisplacement == 1 && pathLength == 1 && r.AllowMeleeRange
        //                                                                   )
        //                                                                ));
        //        //Add to attacking range
        //        if (validRanges.Any(r => r.DealsDamage) && !atkRange.Contains(currCoord))
        //            atkRange.Add(currCoord);
        //        //Add to util range
        //        else if (validRanges.Any(r => !r.DealsDamage) && !utilRange.Contains(currCoord))
        //            utilRange.Add(currCoord);
        //    }

        //    //Don't make these checks on the starting tile
        //    if (!string.IsNullOrEmpty(visitedCoords))
        //    {
        //        //Check if range can continue past this point
        //        if (tile.UnitData.UnitsObstructingItems.Any(u => u.Affiliation.Grouping != parms.AffiliationGrouping))
        //            return;
        //    }

        //    visitedCoords += "_" + currCoord.ToString() + "_";

        //    //Navigate in each cardinal direction, do not repeat tiles in this path
        //    //Left
        //    if (parms.RangeDirection == CompassDirection.Northwest || parms.RangeDirection == CompassDirection.Southwest)
        //    {
        //        ICoordinate left = new Coordinate(Map.Constants.CoordinateFormat, currCoord.X - 1, currCoord.Y);
        //        if (!visitedCoords.Contains("_" + left.ToString() + "_"))
        //            RecurseItemRange(parms, left, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);
        //    }

        //    //Right
        //    if (parms.RangeDirection == CompassDirection.Northeast || parms.RangeDirection == CompassDirection.Southeast)
        //    {
        //        ICoordinate right = new Coordinate(Map.Constants.CoordinateFormat, currCoord.X + 1, currCoord.Y);
        //        if (!visitedCoords.Contains("_" + right.ToString() + "_"))
        //            RecurseItemRange(parms, right, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);
        //    }

        //    //Up
        //    if (parms.RangeDirection == CompassDirection.Northwest || parms.RangeDirection == CompassDirection.Northeast)
        //    {
        //        ICoordinate up = new Coordinate(Map.Constants.CoordinateFormat, currCoord.X, currCoord.Y - 1);
        //        if (!visitedCoords.Contains("_" + up.ToString() + "_"))
        //            RecurseItemRange(parms, up, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);
        //    }

        //    //Down
        //    if (parms.RangeDirection == CompassDirection.Southwest || parms.RangeDirection == CompassDirection.Southeast)
        //    {
        //        ICoordinate down = new Coordinate(Map.Constants.CoordinateFormat, currCoord.X, currCoord.Y + 1);
        //        if (!visitedCoords.Contains("_" + down.ToString() + "_"))
        //            RecurseItemRange(parms, down, remainingRange - 1, visitedCoords, ref atkRange, ref utilRange);

        //    }
        //}

        //private void ApplyWholeMapItemRanges(IUnit unit, List<UnitItemRange> itemRanges, ref List<ICoordinate> atkRange, ref List<ICoordinate> utilRange)
        //{
        //    //Only continue if we have at least one item with a max range of 99.
        //    if (!itemRanges.Any(r => r.MaxRange >= 99))
        //        return;

        //    bool applyAtk = itemRanges.Any(r => r.DealsDamage && r.MaxRange >= 99);
        //    bool applyUtil = itemRanges.Any(r => !r.DealsDamage && r.MaxRange >= 99);

        //    foreach (List<ITile> row in Map.Tiles)
        //    {
        //        foreach (ITile tile in row)
        //        {
        //            //Only exclude tiles that the unit can move to or block items
        //            if (!unit.Ranges.Movement.Contains(tile.Coordinate) && !tile.TerrainType.BlocksItems)
        //            {
        //                if (applyAtk) atkRange.Add(tile.Coordinate);
        //                if (applyUtil) utilRange.Add(tile.Coordinate);
        //            }
        //        }
        //    }

        //    //Remove all relevant ranges from list
        //    //Since we cover the whole map we don't need to address these individually later
        //    if (applyAtk)
        //        itemRanges.RemoveAll(r => r.DealsDamage);

        //    if (applyUtil)
        //        itemRanges.RemoveAll(r => !r.DealsDamage);
        //}

        //private void ApplyNoUnitMovementItemRanges(IUnit unit, List<UnitItemRange> itemRanges, ref List<ICoordinate> atkRange, ref List<ICoordinate> utilRange)
        //{
        //    //Only continue if we have at least one item that can only be used before movement
        //    if (!itemRanges.Any(r => r.CanOnlyUseBeforeMovement))
        //        return;

        //    List<UnitItemRange> noMovementItemRanges = itemRanges.Where(r => r.CanOnlyUseBeforeMovement).ToList();

        //    //Only calculate the item ranges for these items from the unit's origin tiles, not their whole movement range
        //    foreach (ICoordinate coord in unit.Location.OriginTiles.Select(t => t.Coordinate))
        //    {
        //        foreach (CompassDirection direction in RANGE_DIRECTIONS)
        //        {
        //            ItemRangeParameters rangeParms = new ItemRangeParameters(coord, unit.Ranges.Movement, noMovementItemRanges, direction, unit.Affiliation.Grouping);
        //            RecurseItemRange(rangeParms,
        //                             coord,
        //                             rangeParms.LargestRange,
        //                             string.Empty,
        //                             ref atkRange,
        //                             ref utilRange
        //                            );
        //        }
        //    }

        //    //Remove all items from the list so they aren't processed again
        //    itemRanges.RemoveAll(r => r.CanOnlyUseBeforeMovement);
        //}
    }
}
