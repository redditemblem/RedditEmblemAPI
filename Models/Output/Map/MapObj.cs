using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Helpers.Ranges;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Query;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output.Map
{
    #region Interface

    /// <inheritdoc cref="MapObj"/>
    public interface IMapObj
    {
        /// <inheritdoc cref="MapObj.Constants"/>
        MapConstantsConfig Constants { get; }

        /// <inheritdoc cref="MapObj.ChapterPostURL"/>
        string ChapterPostURL { get; }

        /// <inheritdoc cref="MapObj.Segments"/>
        IMapSegment[] Segments { get; }

        /// <inheritdoc cref="MapObj.GetSegmentByCoord(ICoordinate)"/>
        IMapSegment GetSegmentByCoord(ICoordinate coord);

        /// <inheritdoc cref="MapObj.GetTileByCoord(ICoordinate)"/>
        ITile GetTileByCoord(ICoordinate coord);

        /// <inheritdoc cref="MapObj.GetTileByCoord(int, int)"/>
        ITile GetTileByCoord(int x, int y);

        /// <inheritdoc cref="MapObj.GetTilesInRadius(IEnumerable{ITile}, int)"/>
        IEnumerable<ITile> GetTilesInRadius(IEnumerable<ITile> centerTiles, int radius);

        /// <inheritdoc cref="MapObj.GetTilesInRadius(ITile, int)"/>
        IEnumerable<ITile> GetTilesInRadius(ITile centerTile, int radius);
    }

    #endregion Interface

    /// <summary>
    /// Object representing the map.
    /// </summary>
    public class MapObj : IMapObj
    {
        #region Constants

        private static Regex warpGroupRegex = new Regex(@"\(([0-9]+)\)"); //match warp group (ex. "(1)")

        #endregion Constants

        #region Attributes

        /// <summary>
        /// Collection of constant values related to the map.
        /// </summary>
        public MapConstantsConfig Constants { get; private set; }

        /// <summary>
        /// The chapter post's URL.
        /// </summary>
        public string ChapterPostURL { get; private set; }

        /// <summary>
        /// Collection of subsections that make up the complete map.
        /// </summary>
        public IMapSegment[] Segments { get; private set; }

        #endregion Attributes

        #region Constructors

        /// <summary>
        /// Initializes an <c>IImageLoader</c> object and calls the main constructor with all parameters.
        /// </summary>
        public MapObj(MapConfig config, IDictionary<string, ITerrainType> terrainTypes, IDictionary<string, ITileObject> tileObjects)
            : this(config, new ImageLoader(), terrainTypes, tileObjects)
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="MapDataLockedException"></exception>
        /// <exception cref="MapProcessingException"></exception>
        public MapObj(MapConfig config, IImageLoader imageLoader, IDictionary<string, ITerrainType> terrainTypes, IDictionary<string, ITileObject> tileObjects)
        {
            this.Constants = config.Constants;

            //Grab the first matrix row from the MapControls query. Data here should only ever be in one row/column.
            IEnumerable<string> data = config.MapControls.Query.Data.First().Select(v => v.ToString());

            //Validate the map is turned on
            string state = DataParser.OptionalString(data, config.MapControls.MapSwitch, "Map Switch");
            if (!state.Equals("On")) throw new MapDataLockedException();

            string chapterPostURL = DataParser.OptionalString(data, config.MapControls.ChapterPostURL, "Chapter Post URL");
            if (!string.IsNullOrEmpty(chapterPostURL))
                this.ChapterPostURL = DataParser.OptionalString_URL(chapterPostURL, "Chapter Post URL"); //validate URL

            //Build the map segments
            try
            {
                this.Segments = MapSegment.BuildArray(config.MapControls.Segments, config.Constants, data, imageLoader);
                AddTilesToSegments(config.MapTiles, terrainTypes);

                IDictionary<int, ITileObjectInstance> tileObjectInsts = TileObjectInstance.BuildDictionary(config.MapObjects, this, tileObjects);
                AddTileObjectInstancesToSegments(tileObjectInsts);
            }
            catch (Exception ex)
            {
                throw new MapProcessingException(ex);
            }
        }

        #region Segment Construction

        /// <summary>
        /// Uses <paramref name="config"/>'s queried data to contruct tile matrices in each of the map segments.
        /// </summary>
        /// <exception cref="UnexpectedMapHeightException"></exception>
        /// <exception cref="UnexpectedMapWidthException"></exception>
        private void AddTilesToSegments(MapTilesConfig config, IDictionary<string, ITerrainType> terrainTypes)
        {
            IList<IList<object>> rows = config.Query.Data;
            IDictionary<int, List<ITile>> warpGroups = new Dictionary<int, List<ITile>>();

            //The total number of rows in the data set should be equal to the tallest map segment's vertical height
            int maxSegmentHeight = this.Segments.Max(s => s.HeightInTiles);
            if (rows.Count != maxSegmentHeight)
                throw new UnexpectedMapHeightException(rows.Count, maxSegmentHeight, config.Query.Sheet);

            for (int row = 0; row < rows.Count(); row++)
            {
                //One row can contain terrain for multiple segments, separated by empty cells
                IEnumerable<string> totalColumns = rows[row].Select(c => c.ToString()).Where(c => !string.IsNullOrWhiteSpace(c));

                int columnsTaken = 0;
                foreach (IMapSegment segment in this.Segments)
                {
                    //Segments can have varying heights
                    //If we've passed the expected max height of this segment already, skip it
                    if (row >= segment.HeightInTiles) continue;

                    //Grab the range of cells that represent the tiles for the current segment
                    //The start of the range is inclusive, the end is not
                    Range range = new Range(columnsTaken, columnsTaken + segment.WidthInTiles);
                    IEnumerable<string> segmentColumns = totalColumns.Take(range);

                    //Move the start of the range for the next segment
                    columnsTaken += segment.WidthInTiles;

                    //If we reach the end of the populated cells in this row and don't have enough
                    //to match the expected combined segment width, error.
                    if (segmentColumns.Count() != segment.WidthInTiles)
                        throw new UnexpectedMapWidthException(segment.Title, row + 1, segmentColumns.Count(), segment.WidthInTiles, config.Query.Sheet);

                    segment.Tiles[row] = new ITile[segment.WidthInTiles];
                    for (int column = 0; column < segmentColumns.Count(); column++)
                    {
                        string cell = segmentColumns.ElementAt(column);
                        int? warpGroupNum = GetWarpGroupNumber(ref cell);

                        //Calculate the tile's x and y coordinates
                        int x = segment.HorizontalTileRangeWithinMap.Start.Value + column;
                        int y = row + 1;

                        //Create the tile
                        ICoordinate coord = new Coordinate(this.Constants.CoordinateFormat, x, y);
                        ITerrainType terrainType = TerrainType.MatchName(terrainTypes, cell, coord);
                        ITile tile = new Tile(coord, terrainType);

                        //Set the tile's neighbors
                        if (row > 0)
                        {
                            ITile neighbor = segment.Tiles[row - 1][column];
                            tile.Neighbors[(int)CardinalDirection.North] = neighbor;
                            neighbor.Neighbors[(int)CardinalDirection.South] = tile;
                        }
                        if (column > 0)
                        {
                            ITile neighbor = segment.Tiles[row][column - 1];
                            tile.Neighbors[(int)CardinalDirection.West] = neighbor;
                            neighbor.Neighbors[(int)CardinalDirection.East] = tile;
                        }

                        segment.Tiles[row][column] = tile;

                        //If we found a warp group number, add the new tile to a warp group.
                        if (warpGroupNum.HasValue)
                            AddTileToWarpGroups(warpGroups, tile, warpGroupNum.Value);
                    }
                }
            }

            ValidateWarpGroups(warpGroups);
        }

        /// <summary>
        /// Searches <paramref name="terrainTypeText"/> for a warp group number and, if found, returns it as an integer. <paramref name="terrainTypeText"/> will be modified to remove all warp group number syntax.
        /// </summary>
        private int? GetWarpGroupNumber(ref string terrainTypeText)
        {
            int? warpGroupNumber = null;

            Match match = warpGroupRegex.Match(terrainTypeText);
            if (match.Success)
            {
                terrainTypeText = terrainTypeText.Replace(match.Groups[0].Value, string.Empty);
                warpGroupNumber = int.Parse(match.Groups[1].Value);
            }

            terrainTypeText = terrainTypeText.Trim();
            return warpGroupNumber;
        }

        /// <summary>
        /// Adds <paramref name="tile"/> to <paramref name="warpGroupNumber"/> from <paramref name="warpGroups"/>.
        /// </summary>
        /// <exception cref="TerrainTypeNotConfiguredAsWarpException"></exception>
        private void AddTileToWarpGroups(IDictionary<int, List<ITile>> warpGroups, ITile tile, int warpGroupNumber)
        {
            //If the terrain type isn't configured as a warp, error.
            if (tile.TerrainType.WarpType == WarpType.None)
                throw new TerrainTypeNotConfiguredAsWarpException(tile.TerrainType.Name, tile.Coordinate.ToString());

            List<ITile> warpGroup;
            if (!warpGroups.TryGetValue(warpGroupNumber, out warpGroup))
            {
                warpGroup = new List<ITile>();
                warpGroups.Add(warpGroupNumber, warpGroup);
            }

            //Add tile into the group, then link the group to the tile
            warpGroup.Add(tile);

            tile.WarpData.WarpGroup = warpGroup;
            tile.WarpData.WarpGroupNumber = warpGroupNumber;
        }

        ///<summary>
        /// Validates that all <paramref name="warpGroups"/> have at least one valid entrance and exit coordinate each.
        /// </summary>
        /// <exception cref="InvalidWarpGroupException"></exception>
        private void ValidateWarpGroups(IDictionary<int, List<ITile>> warpGroups)
        {
            //All warp groups must have an entrance and an exit
            foreach (int key in warpGroups.Keys)
            {
                List<ITile> group = warpGroups[key];

                IEnumerable<ITile> entrances = group.Where(w => w.TerrainType.WarpType == WarpType.Entrance || w.TerrainType.WarpType == WarpType.Dual);
                IEnumerable<ITile> exits = group.Where(w => w.TerrainType.WarpType == WarpType.Exit || w.TerrainType.WarpType == WarpType.Dual);

                //If we do not have at least one distinct entrance and exit, error
                if (!entrances.Any()
                 || !exits.Any()
                 || entrances.Select(e => e.Coordinate).Union(exits.Select(e => e.Coordinate)).Distinct().Count() < 2)
                    throw new InvalidWarpGroupException(key.ToString());
            }
        }

        /// <summary>
        /// Partitions out <paramref name="tileObjectInsts"/> to the correct map segments.
        /// </summary>
        private void AddTileObjectInstancesToSegments(IDictionary<int, ITileObjectInstance> tileObjectInsts)
        {
            var groupedBySegment = tileObjectInsts.GroupBy(to => GetSegmentByCoord(to.Value.AnchorCoordinate));
            foreach (var group in groupedBySegment)
                group.Key.TileObjectInstances = group.ToDictionary();
        }

        #endregion Segment Construction

        #endregion Constructors

        #region Tile Functions

        /// <summary>
        /// Finds and returns the map segment that contains <paramref name="coord"/>.
        /// </summary>
        /// <exception cref="TileOutOfBoundsException"></exception>
        public IMapSegment GetSegmentByCoord(ICoordinate coord)
        {
            IMapSegment? segment = this.Segments.FirstOrDefault(s => s.CoordinateFallsWithinRange(coord));
            if (segment is null) throw new TileOutOfBoundsException(coord);

            return segment;
        }

        /// <summary>
        /// Finds and returns the tile with matching coordinates to <paramref name="coord"/>.
        /// </summary>
        public ITile GetTileByCoord(ICoordinate coord)
        {
            return GetSegmentByCoord(coord).GetTileByCoord(coord);
        }

        /// <summary>
        /// Finds and returns the tile with matching coordinates to {<paramref name="x"/>, <paramref name="y"/>}.
        /// </summary>
        public ITile GetTileByCoord(int x, int y)
        {
            ICoordinate coord = new Coordinate(this.Constants.CoordinateFormat, x, y);
            return GetTileByCoord(coord);
        }

        /// <summary>
        /// Returns a set of distinct tiles that are within <paramref name="radius"/> tiles of the <paramref name="centerTiles"/>.
        /// </summary>
        public IEnumerable<ITile> GetTilesInRadius(IEnumerable<ITile> centerTiles, int radius)
        {
            return centerTiles.SelectMany(t => GetTilesInRadius(t, radius)).Except(centerTiles);
        }

        /// <summary>
        /// Returns a set of distinct tiles that are within <paramref name="radius"/> tiles of the <paramref name="centerTile"/>.
        /// </summary>
        public IEnumerable<ITile> GetTilesInRadius(ITile centerTile, int radius)
        {
            return GetSegmentByCoord(centerTile.Coordinate).GetTilesInRadius(centerTile.Coordinate, radius);
        }

        #endregion Tile Functions
    }
}
