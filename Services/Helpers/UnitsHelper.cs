using NCalc;
using RedditEmblemAPI.Models.Common;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public class UnitsHelper
    {
        /// <summary>
        /// Parses Google Sheets data matrix to return a list of Unit output objects.
        /// </summary>
        /// <param name="data">Matrix of sheet Value values representing unit data</param>
        /// <param name="config">Parsed JSON configuration mapping Values to output</param>
        /// <returns></returns>
        public static IList<Unit> Process(UnitsConfig config, SystemData systemData, List<List<Tile>> map)
        {
            IList<Unit> units = new List<Unit>();

            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    //Convert objects to strings
                    IList<string> unit = row.Select(r => r.ToString()).ToList();

                    //Skip blank units
                    if (string.IsNullOrEmpty(unit.ElementAtOrDefault(config.UnitName)))
                        continue;

                    Unit temp = new Unit(config, unit, systemData);
                    AddUnitToMap(temp, map);

                    units.Add(temp);
                }
                catch (Exception ex)
                {
                    throw new UnitProcessingException(row.ElementAtOrDefault(config.UnitName).ToString(), ex);
                }
            }

            return units;
        }

        private static void AddUnitToMap(Unit unit, List<List<Tile>> map)
        {
            //Ignore hidden units
            if (unit.Coordinates.X < 1 || unit.Coordinates.Y < 1)
                return;

            //Find tile corresponsing to units coordinates
            IList<Tile> row = map.ElementAtOrDefault(unit.Coordinates.Y - 1) ?? throw new UnitTileOutOfBoundsException(unit.Coordinates);
            Tile tile = row.ElementAtOrDefault(unit.Coordinates.X - 1) ?? throw new UnitTileOutOfBoundsException(unit.Coordinates);

            //Two way bind the unit and tile objects
            unit.AnchorTile = tile;
            tile.Unit = unit;
            tile.IsUnitAnchor = true;

            unit.MovementRange.Add(new Coordinate(tile.Coordinate));

            if (unit.UnitSize > 1)
            {
                //Calculate origin tile for multi-tile units
                int anchorOffset = (int)Math.Ceiling(unit.UnitSize / 2.0m) - 1;

                for(int y = 0; y < unit.UnitSize; y++)
                {
                    for (int x = 0; x < unit.UnitSize; x++)
                    {
                        IList<Tile> intersectRow = map.ElementAtOrDefault(unit.Coordinates.Y + y - 1) ?? throw new UnitTileOutOfBoundsException(unit.Coordinates.X + x, unit.Coordinates.Y + y);
                        Tile intersectTile = intersectRow.ElementAtOrDefault(unit.Coordinates.X + x - 1) ?? throw new UnitTileOutOfBoundsException(unit.Coordinates.X + x, unit.Coordinates.Y + y);

                        intersectTile.Unit = unit;
                        if(!unit.MovementRange.Contains(intersectTile.Coordinate))
                            unit.MovementRange.Add(intersectTile.Coordinate);
                        
                        if (x == anchorOffset && y == anchorOffset)
                        {
                            unit.OriginTile = intersectTile;
                            intersectTile.IsUnitOrigin = true;
                        }
                    }
                }
            }
            else
            {
                //Single tile units have their anchor and origin in the same place.
                unit.OriginTile = tile;
                tile.IsUnitOrigin = true;
            }   
        }
    }
}
