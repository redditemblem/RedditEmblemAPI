using Antlr4.Runtime.Misc;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Helpers.Ranges.Items
{
    public class ItemRangeCalculator
    {
        #region Setup

        private IReadOnlyDictionary<OrdinalDirection, CardinalDirection[]> RANGE_DIRECTIONS;

        private IEnumerable<IUnit> Units;
        private IMapObj Map;

        public ItemRangeCalculator(IMapObj map, IEnumerable<IUnit> units)
        {
            this.RANGE_DIRECTIONS = CreateRangeDirectionDictionary();

            this.Map = map;
            this.Units = units;
        }

        //We access this dictionary a lot, so the faster lookup speed should be worth the overhead of freezing it.
        private IReadOnlyDictionary<OrdinalDirection, CardinalDirection[]> CreateRangeDirectionDictionary()
        {
             var dictionary = new Dictionary<OrdinalDirection, CardinalDirection[]> {
                { OrdinalDirection.Northeast, new CardinalDirection[2]{ CardinalDirection.North, CardinalDirection.East } },
                { OrdinalDirection.Southeast, new CardinalDirection[2]{ CardinalDirection.South, CardinalDirection.East } },
                { OrdinalDirection.Southwest, new CardinalDirection[2]{ CardinalDirection.South, CardinalDirection.West } },
                { OrdinalDirection.Northwest, new CardinalDirection[2]{ CardinalDirection.North, CardinalDirection.West } }
            };

            return dictionary.ToFrozenDictionary();
        }

        #endregion Setup

        public void CalculateTileObjectRanges()
        {
            foreach (ITileObjectInstance tileObjInst in Map.Segments.SelectMany(s => s.TileObjectInstances.Values))
            {
                //If tile object does not have a configured range, skip it.
                if (tileObjInst.TileObject.Range.Minimum < 1 || tileObjInst.TileObject.Range.Maximum < 1)
                    continue;

                IList<ICoordinate> atkRange = new List<ICoordinate>();
                IList<ICoordinate> utilRange = new List<ICoordinate>();

                //Transpose item data into the struct we're using for recursion
                List<UnitItemRange> ranges = new List<UnitItemRange> { new UnitItemRange(tileObjInst.TileObject.Range.Minimum, tileObjInst.TileObject.Range.Maximum, ItemRangeShape.Standard, false, true, false) };

                foreach (ITile originTile in tileObjInst.OriginTiles)
                {
                    foreach (OrdinalDirection direction in RANGE_DIRECTIONS.Keys)
                    {
                        ItemRangeParameters rangeParms = new ItemRangeParameters(originTile.Coordinate, tileObjInst.OriginTiles.Select(ot => ot.Coordinate).ToList(), ranges, direction, 0);
                        RecurseItemRange(rangeParms,
                                         originTile,
                                         rangeParms.LargestRange,
                                         ref atkRange,
                                         ref utilRange
                                        );
                    }
                }

                tileObjInst.AttackRange = atkRange;
            }
        }

        public void CalculateUnitItemRanges()
        {
            foreach (IUnit unit in Units)
            {
                try
                {
                    //Ignore hidden units
                    if (!unit.Location.OriginTiles.Any())
                        continue;

                    IList<ICoordinate> attackRange, utilityRange;
                    CalculateUnitItemRange(unit, out attackRange, out utilityRange);

                    unit.Ranges.Attack = attackRange;
                    unit.Ranges.Utility = utilityRange;
                }
                catch (Exception ex)
                {
                    throw new RangeCalculationException(unit, ex);
                }
            }
        }

        /// <summary>
        /// Calculates all item ranges for <paramref name="unit"/>.
        /// </summary>
        private void CalculateUnitItemRange(IUnit unit, out IList<ICoordinate> attackRange, out IList<ICoordinate> utilityRange)
        {
            attackRange = new List<ICoordinate>();
            utilityRange = new List<ICoordinate>();

            //Transpose item data into the struct we're using for recursion
            IEnumerable<UnitItemRange> itemRanges = SelectEligibleUnitInventoryItems(unit.Inventory.GetAllItems());

            //If unit is engaged with an emblem, include its items in the range as well
            if (unit.Emblem is not null && unit.Emblem.IsEngaged)
                itemRanges = itemRanges.Union(SelectEligibleUnitInventoryItems(unit.Emblem.EngageWeapons));

            //Check for special case ranges
            ApplyWholeMapItemRanges(unit, ref itemRanges, ref attackRange, ref utilityRange);
            ApplyNoUnitMovementItemRanges(unit, ref itemRanges, ref attackRange, ref utilityRange);

            //If we've got no ranges to process, leave.
            if (!itemRanges.Any())
                return;

            //Calculate any remaining ranges
            foreach (ICoordinate coord in unit.Ranges.Movement)
            {
                ITile tile = Map.GetTileByCoord(coord);

                foreach (OrdinalDirection direction in RANGE_DIRECTIONS.Keys)
                {
                    ItemRangeParameters rangeParms = new ItemRangeParameters(coord, unit.Ranges.Movement, itemRanges, direction, unit.Affiliation.Grouping);
                    RecurseItemRange(rangeParms, tile, rangeParms.LargestRange, ref attackRange, ref utilityRange);
                }
            }
        }

        /// <summary>
        /// Returns item range parameter structs for all <paramref name="items"/> that qualify to have a range calculated.
        /// </summary>
        private IEnumerable<UnitItemRange> SelectEligibleUnitInventoryItems(IEnumerable<IUnitInventoryItem> items)
        {
            return items.Where(i => i.CanEquip
                                && !i.IsUsePrevented
                                && !i.MaxRangeExceedsCalculationLimit
                                && (i.MinRange.FinalValue > 0 || i.MaxRange.FinalValue > 0))
                        .Select(i => new UnitItemRange(i.MinRange.FinalValue,
                                                       i.MaxRange.FinalValue,
                                                       i.Item.Range.Shape,
                                                       i.Item.Range.CanOnlyUseBeforeMovement,
                                                       i.Item.DealsDamage,
                                                       i.AllowMeleeRange));
        }

        private void RecurseItemRange(ItemRangeParameters parms, ITile tile, int remainingRange, ref IList<ICoordinate> attackRange, ref IList<ICoordinate> utilityRange)
        {
            ICoordinate coordinate = tile.Coordinate;
            int horzDisplacement = Math.Abs(coordinate.X - parms.StartingCoordinate.X);
            int verticalDisplacement = Math.Abs(coordinate.Y - parms.StartingCoordinate.Y);
            int totalDisplacement = horzDisplacement + verticalDisplacement;

            //Check for items that can reach this tile
            if (!parms.IgnoreTiles.Contains(coordinate))
            {
                List<UnitItemRange> validRanges = GetQualifyingRangesBasedOnDisplacement(parms, horzDisplacement, verticalDisplacement, totalDisplacement);

                //Add coordinate to range sets
                if (validRanges.Any(r => r.DealsDamage) && !attackRange.Contains(coordinate))
                    attackRange.Add(coordinate);
                else if (validRanges.Any(r => !r.DealsDamage) && !utilityRange.Contains(coordinate))
                    utilityRange.Add(coordinate);
            }

            //Check if range can continue past this point
            //Skip this check on the starting tile
            if (totalDisplacement > 0 && tile.UnitData.UnitsObstructingItems.Any(u => u.Affiliation.Grouping != parms.AffiliationGrouping))
                return;

            //If we're out of range, don't visit neighbors.
            if (remainingRange < 1)
                return;

            //Navigate in the appropriate cardinal directions to neighboring tiles
            CardinalDirection[] directions = RANGE_DIRECTIONS[parms.RangeDirection];
            foreach(CardinalDirection direction in directions)
            {
                ITile neighbor = tile.Neighbors[(int)direction];
                if (neighbor is null || neighbor.TerrainType.BlocksItems) continue;

                RecurseItemRange(parms, neighbor, remainingRange - 1, ref attackRange, ref utilityRange);
            }
        }

        /// <summary>
        /// Determines which of the item ranges from <paramref name="parms"/> are able to hit a tile with <paramref name="horizontalDisplacement"/>, <paramref name="verticalDisplacement"/>, and <paramref name="totalDisplacement"/> from the starting tile.
        /// </summary>
        private List<UnitItemRange> GetQualifyingRangesBasedOnDisplacement(ItemRangeParameters parms, int horizontalDisplacement, int verticalDisplacement, int totalDisplacement)
        {
            List<UnitItemRange> validRanges = new List<UnitItemRange>();

            if (parms.ContainsRangeShape[ItemRangeShape.Standard])
            {
                validRanges.AddRange(parms.Ranges.Where(r => r.Shape == ItemRangeShape.Standard)
                                                 .Where(r => r.MinRange <= totalDisplacement && r.MaxRange >= totalDisplacement));
            }

            if (parms.ContainsRangeShape[ItemRangeShape.Square])
            {
                validRanges.AddRange(parms.Ranges.Where(r => r.Shape == ItemRangeShape.Square)
                                                 .Where(r => (r.MinRange <= verticalDisplacement || r.MinRange <= horizontalDisplacement)
                                                           && r.MaxRange >= verticalDisplacement
                                                           && r.MaxRange >= horizontalDisplacement
                                                       ));
            }

            //Check if we're on a straight line from the starting tile
            if ((horizontalDisplacement == 0 || verticalDisplacement == 0) && (parms.ContainsRangeShape[ItemRangeShape.Cross] || parms.ContainsRangeShape[ItemRangeShape.Star]))
            {
                validRanges.AddRange(parms.Ranges.Where(r => r.Shape == ItemRangeShape.Cross || r.Shape == ItemRangeShape.Star)
                                                 .Where(r => (horizontalDisplacement == 0 && r.MinRange <= verticalDisplacement && r.MaxRange >= verticalDisplacement)
                                                          || (verticalDisplacement == 0 && r.MinRange <= horizontalDisplacement && r.MaxRange >= horizontalDisplacement)
                                                       ));
            }

            //Check if we're on a diagonal from the starting tile
            if (horizontalDisplacement == verticalDisplacement && (parms.ContainsRangeShape[ItemRangeShape.Saltire] || parms.ContainsRangeShape[ItemRangeShape.Star]))
            {
                validRanges.AddRange(parms.Ranges.Where(r => r.Shape == ItemRangeShape.Saltire || r.Shape == ItemRangeShape.Star)
                                                 .Where(r => r.MinRange <= horizontalDisplacement && r.MaxRange >= horizontalDisplacement));
            }

            //Check if we're only one tile away from the starting tile
            if (totalDisplacement == 1)
            {
                validRanges.AddRange(parms.Ranges.Where(r => r.AllowMeleeRange));
            }

            return validRanges;
        }

        private void ApplyWholeMapItemRanges(IUnit unit, ref IEnumerable<UnitItemRange> itemRanges, ref IList<ICoordinate> attackRange, ref IList<ICoordinate> utilityRange)
        {
            //Only continue if we have at least one item with a max range of 99.
            if (!itemRanges.Any(r => r.MaxRange >= 99))
                return;

            bool applyAtk = itemRanges.Any(r => r.DealsDamage && r.MaxRange >= 99);
            bool applyUtil = itemRanges.Any(r => !r.DealsDamage && r.MaxRange >= 99);

            //Only cover the map segment the unit is currently located on
            IMapSegment segment = Map.GetSegmentByCoord(unit.Location.AnchorTile.Coordinate);

            foreach (ITile[] row in segment.Tiles)
            {
                foreach (ITile tile in row)
                {
                    //Only exclude tiles that the unit can move to or block items
                    if (!unit.Ranges.Movement.Contains(tile.Coordinate) && !tile.TerrainType.BlocksItems)
                    {
                        if (applyAtk) attackRange.Add(tile.Coordinate);
                        if (applyUtil) utilityRange.Add(tile.Coordinate);
                    }
                }
            }

            //Remove all relevant ranges from list
            //Since we cover the whole map we don't need to address these individually later
            if (applyAtk) itemRanges = itemRanges.Where(r => !r.DealsDamage);
            if (applyUtil) itemRanges = itemRanges.Where(r => r.DealsDamage);
        }

        private void ApplyNoUnitMovementItemRanges(IUnit unit, ref IEnumerable<UnitItemRange> itemRanges, ref IList<ICoordinate> attackRange, ref IList<ICoordinate> utilityRange)
        {
            //Only continue if we have at least one item that can only be used before movement
            IEnumerable<UnitItemRange> noMovementItemRanges = itemRanges.Where(r => r.CanOnlyUseBeforeMovement);
            if (noMovementItemRanges.Count() < 1)
                return;

            //Only calculate the item ranges for these items from the unit's origin tiles, not their whole movement range
            foreach (ITile tile in unit.Location.OriginTiles)
            {
                foreach (OrdinalDirection direction in RANGE_DIRECTIONS.Keys)
                {
                    ItemRangeParameters rangeParms = new ItemRangeParameters(tile.Coordinate, unit.Ranges.Movement, noMovementItemRanges, direction, unit.Affiliation.Grouping);
                    RecurseItemRange(rangeParms,
                                     tile,
                                     rangeParms.LargestRange,
                                     ref attackRange,
                                     ref utilityRange
                                    );
                }
            }

            //Remove all items from the list so they aren't processed again
            itemRanges = itemRanges.Where(r => !r.CanOnlyUseBeforeMovement);
        }
    }
}