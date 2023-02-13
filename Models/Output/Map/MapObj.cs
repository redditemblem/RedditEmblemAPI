using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Query;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Services.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RedditEmblemAPI.Models.Output.Map
{
    /// <summary>
    /// Object representing the map.
    /// </summary>
    public class MapObj
    {
        #region Attributes

        /// <summary>
        /// The map's image URL.
        /// </summary>
        public string MapImageURL { get; set; }

        /// <summary>
        /// The chapter post's URL.
        /// </summary>
        public string ChapterPostURL { get; set; }

        /// <summary>
        /// The height of the map image in pixels.
        /// </summary>
        [JsonProperty]
        private int ImageHeight { get; set; }

        /// <summary>
        /// The width of the map image in pixels.
        /// </summary>
        [JsonProperty]
        private int ImageWidth { get; set; }

        /// <summary>
        /// The height of the map in number of tiles.
        /// </summary>
        [JsonIgnore]
        public int MapHeightInTiles { get; private set; }

        /// <summary>
        /// The width of the map in number of tiles.
        /// </summary>
        [JsonIgnore]
        public int MapWidthInTiles { get; private set; }

        /// <summary>
        /// Collection of constant values for doing calculations.
        /// </summary>
        public MapConstantsConfig Constants { get; set; }

        /// <summary>
        /// Matrix of map tiles.
        /// </summary>
        public List<List<Tile>> Tiles { get; set; }

        /// <summary>
        /// Dictionary of tile object instances present on the map.
        /// </summary>
        public Dictionary<int, TileObjectInstance> TileObjectInstances { get; set; }

        #endregion

        #region Constants

        private static Regex warpGroupRegex = new Regex(@"\(([0-9]+)\)"); //match warp group (ex. "(1)")

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes all of the <c>Map</c>'s attributes and builds its <c>Tiles</c> matrix.
        /// </summary>
        /// <exception cref="MapDataLockedException"></exception>
        /// <exception cref="MapImageURLNotFoundException"></exception>
        public MapObj(MapConfig config, IDictionary<string, TerrainType> terrainTypes, IDictionary<string, TileObject> tileObjects)
        {
            this.Constants = config.Constants;

            //Grab the first matrix row from the MapControls query. Data here should only ever be in one row/column.
            IList<object> values = config.MapControls.Query.Data.First();

            //Validate the map is turned on
            if ((values.ElementAtOrDefault(config.MapControls.MapSwitch) ?? "Off").ToString() != "On")
                throw new MapDataLockedException();

            //Validate we have a map image
            string mapImageURL = (values.ElementAtOrDefault(config.MapControls.MapImageURL) ?? string.Empty).ToString();
            if (string.IsNullOrEmpty(mapImageURL))
                throw new MapImageURLNotFoundException(config.MapControls.Query.Sheet);
            DataParser.String_URL(mapImageURL, "Map Image URL"); //validate URL
            this.MapImageURL = mapImageURL;

            this.ChapterPostURL = (values.ElementAtOrDefault(config.MapControls.ChapterPostURL) ?? string.Empty).ToString();
            DataParser.OptionalString_URL(this.ChapterPostURL, "Chapter Post URL"); //validate URL

            GetMapDimensionsFromImage();

            //Build tile matrix
            this.Tiles = new List<List<Tile>>();
            BuildTiles(config.MapTiles, terrainTypes);

            //If we have tile objects configured, add those to the map
            this.TileObjectInstances = new Dictionary<int, TileObjectInstance>();
            if (config.MapObjects != null)
                AddTileObjectsToTiles(config.MapObjects, tileObjects);
        }

        #endregion

        /// <summary>
        /// Calculates the expected height/width of the map in # of tiles based on the dimensions the image loaded from <c>MapImageURL</c>.
        /// </summary>
        private void GetMapDimensionsFromImage()
        {
            int tileHeight;
            int tileWidth;

            Task<byte[]> imageData = new HttpClient().GetByteArrayAsync(this.MapImageURL);
            imageData.Wait();

            using (MemoryStream imgStream = new MemoryStream(imageData.Result))
            using (SKManagedStream inputStream = new SKManagedStream(imgStream))
            using (SKBitmap img = SKBitmap.Decode(inputStream))
            {
                this.ImageHeight = img.Height;
                this.ImageWidth = img.Width;
            }

            tileHeight = (int)Math.Floor((decimal)this.ImageHeight / (this.Constants.TileSize + this.Constants.TileSpacing));
            tileWidth = (int)Math.Floor((decimal)this.ImageWidth / (this.Constants.TileSize + this.Constants.TileSpacing));

            if (this.Constants.HasHeaderTopLeft)
            {
                tileHeight -= 1;
                tileWidth -= 1;
            }

            if (this.Constants.HasHeaderBottomRight)
            {
                tileHeight -= 1;
                tileWidth -= 1;
            }

            this.MapHeightInTiles = tileHeight;
            this.MapWidthInTiles = tileWidth;
        }

        /// <summary>
        /// Uses the data from <paramref name="config"/>'s query to contruct the <c>Tiles</c> matrix.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="terrainTypes"></param>
        /// <exception cref="MapProcessingException"></exception>
        private void BuildTiles(MapTilesConfig config, IDictionary<string, TerrainType> terrainTypes)
        {
            int x = 1;
            int y = 1;

            try
            {
                IList<IList<object>> tileData = config.Query.Data;
                IDictionary<int, List<Tile>> warpGroups = new Dictionary<int, List<Tile>>();

                if (tileData.Count != this.MapHeightInTiles)
                    throw new UnexpectedMapHeightException(tileData.Count, this.MapHeightInTiles, config.Query.Sheet);

                foreach (List<object> row in tileData)
                {
                    if (row.Count != this.MapWidthInTiles)
                        throw new UnexpectedMapWidthException(row.Count, this.MapWidthInTiles, config.Query.Sheet);

                    List<Tile> currentRow = new List<Tile>();
                    foreach (object tile in row)
                    {
                        string t = tile.ToString().Trim();

                        //Search for warp group number
                        int warpGroupNum = 0;
                        Match match = warpGroupRegex.Match(t);
                        if (match.Success)
                        {
                            t = t.Replace(match.Groups[0].Value, string.Empty).Trim();
                            warpGroupNum = int.Parse(match.Groups[1].Value);
                        }

                        //Match on tile's terrain type
                        Coordinate coord = new Coordinate(this.Constants.CoordinateFormat, x, y);
                        TerrainType type = TerrainType.MatchName(terrainTypes, t, coord);
                        Tile temp = new Tile(coord, type);

                        //If we found a warp group number, add the new tile to a warp group.
                        if (warpGroupNum > 0)
                        {
                            //If the terrain type isn't configured as a warp, error.
                            if (type.WarpType == WarpType.None)
                                throw new TerrainTypeNotConfiguredAsWarpException(t, tile.ToString());

                            List<Tile> warpGroup;
                            if (!warpGroups.TryGetValue(warpGroupNum, out warpGroup))
                            {
                                warpGroup = new List<Tile>();
                                warpGroups.Add(warpGroupNum, warpGroup);
                            }

                            //Two-way bind warp group data
                            warpGroup.Add(temp);
                            temp.WarpData.WarpGroup = warpGroup;
                            temp.WarpData.WarpGroupNumber = warpGroupNum;
                        }

                        currentRow.Add(temp);
                        x++;
                    }

                    this.Tiles.Add(currentRow);

                    x = 1;
                    y++;
                }

                //Validate all warp groups for entrances/exits
                foreach (int key in warpGroups.Keys)
                {
                    List<Tile> group = warpGroups[key];

                    IEnumerable<Tile> entrances = group.Where(w => w.TerrainTypeObj.WarpType == WarpType.Entrance || w.TerrainTypeObj.WarpType == WarpType.Dual);
                    IEnumerable<Tile> exits = group.Where(w => w.TerrainTypeObj.WarpType == WarpType.Exit || w.TerrainTypeObj.WarpType == WarpType.Dual);

                    //If we do not have at least one distinct entrance and exit
                    if (  !entrances.Any()
                       || !exits.Any()
                       || entrances.Select(e => e.Coordinate).Union(exits.Select(e => e.Coordinate)).Distinct().Count() < 2
                        )
                        throw new InvalidWarpGroupException(key.ToString());
                }

            }
            catch (Exception ex)
            {
                throw new MapProcessingException(ex);
            }
        }

        /// <summary>
        /// Uses the data from <paramref name="config"/>'s query to apply <c>TileObjects</c>s to the map.
        /// </summary>
        private void AddTileObjectsToTiles(MapObjectsConfig config, IDictionary<string, TileObject> tileObjects)
        {
            try
            {
                IList<IList<object>> tileData = config.Query.Data;

                //Not every tile has to be mapped on this page, so just ensure we don't exceed the existing map size.
                if (tileData.Count > this.Tiles.Count)
                    throw new UnexpectedMapHeightException(tileData.Count, this.Tiles.Count, config.Query.Sheet);

                int idIterator = 1;
                for (int r = 0; r < tileData.Count; r++)
                {
                    IList<object> row = tileData[r];
                    IList<Tile> tiles = this.Tiles[r];

                    if (row.Count > tiles.Count)
                        throw new UnexpectedMapWidthException(row.Count, tiles.Count, config.Query.Sheet);

                    for (int c = 0; c < row.Count; c++)
                    {
                        string cell = row[c].ToString();

                        //Skip empty cells
                        if (string.IsNullOrEmpty(cell))
                            continue;

                        foreach (string value in cell.Split(","))
                        {
                            //Skip any empty strings
                            if (string.IsNullOrWhiteSpace(value)) continue;

                            TileObject tileObj = TileObject.MatchName(tileObjects, value.Trim(), tiles[c].Coordinate);
                            TileObjectInstance tileObjInst = new TileObjectInstance(idIterator++, tileObj);

                            //3-way bind
                            this.TileObjectInstances.Add(tileObjInst.ID, tileObjInst);
                            tiles[c].TileObjects.Add(tileObjInst);
                            tileObjInst.OriginTiles.Add(tiles[c]);

                            //Set all tiles for multi-tile effects
                            if (tileObj.Size > 1)
                            {
                                for (int r2 = r; r2 < r + tileObj.Size; r2++)
                                {
                                    for (int c2 = c; c2 < c + tileObj.Size; c2++)
                                    {
                                        //Skip the starting tile
                                        if (r2 == r && c2 == c)
                                            continue;

                                        if (r2 >= this.Tiles.Count)
                                            throw new TileOutOfBoundsException(new Coordinate(this.Constants.CoordinateFormat, r2, c2));

                                        List<Tile> tiles2 = this.Tiles[r2];

                                        if (c2 >= tiles2.Count)
                                            throw new TileOutOfBoundsException(new Coordinate(this.Constants.CoordinateFormat, r2, c2));

                                        //2-way bind
                                        tiles2[c2].TileObjects.Add(tileObjInst);
                                        tileObjInst.OriginTiles.Add(tiles2[c2]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new MapProcessingException(ex);
            }
        }

        #region Tile Functions

        /// <summary>
        /// Fetches the tile with matching coordinates to <paramref name="coord"/>.
        /// </summary>
        /// <exception cref="TileOutOfBoundsException"></exception>
        public Tile GetTileByCoord(Coordinate coord)
        {
            return GetTileByCoord(coord.X, coord.Y);
        }

        /// <summary>
        /// Fetches the tile with matching coordinates to <paramref name="x"/>,<paramref name="y"/>.
        /// </summary>
        /// <exception cref="TileOutOfBoundsException"></exception>
        public Tile GetTileByCoord(int x, int y)
        {
            List<Tile> row = this.Tiles.ElementAtOrDefault<List<Tile>>(y - 1) ?? throw new TileOutOfBoundsException(new Coordinate(this.Constants.CoordinateFormat, x, y));
            Tile column = row.ElementAtOrDefault<Tile>(x - 1) ?? throw new TileOutOfBoundsException(new Coordinate(this.Constants.CoordinateFormat, x, y));

            return column;
        }

        /// <summary>
        /// Returns a list of distinct tiles that are within <paramref name="radius"/> tiles of the <paramref name="centerTiles"/>.
        /// </summary>
        public List<Tile> GetTilesInRadius(List<Tile> centerTiles, int radius)
        {
            return centerTiles.SelectMany(t => GetTilesInRadius(t.Coordinate, radius)).Except(centerTiles).ToList();
        }

        /// <summary>
        /// Returns a list of distinct tiles that are within <paramref name="radius"/> tiles of the <paramref name="centerTile"/>.
        /// </summary>
        public List<Tile> GetTilesInRadius(Tile centerTile, int radius)
        {
            return GetTilesInRadius(centerTile.Coordinate, radius);
        }

        /// <summary>
        /// Returns a list of distinct tiles that are within <paramref name="radius"/> tiles of the <paramref name="center"/>.
        /// </summary>
        public List<Tile> GetTilesInRadius(Coordinate center, int radius)
        {
            List<Tile> temp = new List<Tile>();

            for (int x = 0; x <= radius; x++)
            {
                for (int y = 0; y <= radius; y++)
                {
                    if (x == 0 && y == 0) continue; //ignore origin
                    if (x + y > radius) continue; //if the total displacement is greater than the radius, stop

                    //If fetching any tile fails, we still want to continue execution
                    try { temp.Add(GetTileByCoord(center.X + x, center.Y + y)); } catch (TileOutOfBoundsException) { }
                    try { temp.Add(GetTileByCoord(center.X - x, center.Y + y)); } catch (TileOutOfBoundsException) { }
                    try { temp.Add(GetTileByCoord(center.X + x, center.Y - y)); } catch (TileOutOfBoundsException) { }
                    try { temp.Add(GetTileByCoord(center.X - x, center.Y - y)); } catch (TileOutOfBoundsException) { }
                }
            }

            //Return distinct list of tiles
            return temp.Distinct().ToList();
        }

        #endregion
    }
}
