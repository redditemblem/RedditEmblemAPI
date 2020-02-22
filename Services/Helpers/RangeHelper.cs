using RedditEmblemAPI.Models.Common;
using RedditEmblemAPI.Models.Exceptions;
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
                    RecurseUnitRange(unit, unit.Stats.GetValueOrDefault("Mov").FinalValue, unit.Coordinates, new List<Coordinate>());

                    //Find the items with minimum and maximum attack range
                    Item minAtkRange = unit.Inventory.Where(i => i != null && i.DealsDamage && i.UtilizedStat.Length > 0).OrderBy(i => i.Range.Minimum).FirstOrDefault();
                    Item maxAtkRange = unit.Inventory.Where(i => i != null && i.DealsDamage && i.UtilizedStat.Length > 0).OrderBy(i => i.Range.Maximum).FirstOrDefault();

                    //Find the items with minimum and maximum utility range
                    Item minUtilRange = unit.Inventory.Where(i => i != null && !i.DealsDamage && i.UtilizedStat.Length > 0).OrderBy(i => i.Range.Minimum).FirstOrDefault();
                    Item maxUtilRange = unit.Inventory.Where(i => i != null && !i.DealsDamage && i.UtilizedStat.Length > 0).OrderBy(i => i.Range.Maximum).FirstOrDefault();

                    IList<Coordinate> atkRange = new List<Coordinate>();
                    IList<Coordinate> utilRange = new List<Coordinate>();
                    foreach (Coordinate coord in unit.MovementRange)
                    {
                        //Calculate attack range
                        RecurseItemRange( unit,
                                          coord,
                                         (minAtkRange != null ? minAtkRange.Range.Minimum : 0),
                                         (maxAtkRange != null ? maxAtkRange.Range.Maximum : 0),
                                          new List<Coordinate>(),
                                          ref atkRange
                                        );

                        //Calculate utility range
                        RecurseItemRange( unit,
                                          coord,
                                         (minUtilRange != null ? minUtilRange.Range.Minimum : 0),
                                         (maxUtilRange != null ? maxUtilRange.Range.Maximum : 0),
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
            if (remainingMov < 0)
                return;

            visitedCoords.Add(currCoord);

            if (!unit.MovementRange.Contains(currCoord))
                unit.MovementRange.Add(currCoord);
        }

        private void RecurseItemRange(Unit unit, Coordinate currCoord, int remainingMinRange, int remainingMaxRange, IList<Coordinate> visitedCoords, ref IList<Coordinate> itemRange)
        {
            //Base case
            //Don't exceed the maximum range and don't go off the map
            if (   remainingMaxRange < 0 
                || currCoord.X < 1
                || currCoord.Y < 1
                || currCoord.X >= this.Tiles.First().Count
                || currCoord.Y >= this.Tiles.Count
               )
                return;

            if(remainingMinRange <= 0)
            {
                visitedCoords.Add(currCoord);

                if (!unit.MovementRange.Contains(currCoord) && !itemRange.Contains(currCoord))
                    itemRange.Add(currCoord);
            }

            //Left
            RecurseItemRange(unit, new Coordinate(currCoord.X - 1, currCoord.Y), remainingMinRange - 1, remainingMaxRange - 1, visitedCoords, ref itemRange);

            //Right
            RecurseItemRange(unit, new Coordinate(currCoord.X + 1, currCoord.Y), remainingMinRange - 1, remainingMaxRange - 1, visitedCoords, ref itemRange);
            
            //Up
            RecurseItemRange(unit, new Coordinate(currCoord.X, currCoord.Y - 1), remainingMinRange - 1, remainingMaxRange - 1, visitedCoords, ref itemRange);
            
            //Down
            RecurseItemRange(unit, new Coordinate(currCoord.X, currCoord.Y + 1), remainingMinRange - 1, remainingMaxRange - 1, visitedCoords, ref itemRange);
        }

        private Tile FetchTileByCoord(Coordinate coord)
        {
            IList<Tile> row = this.Tiles.ElementAtOrDefault(coord.Y - 1);
            if (row == null)
                return null;
            return row.ElementAtOrDefault(coord.X - 1);
        }
    }
}
