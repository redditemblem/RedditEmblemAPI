using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Query;
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
    #region Interface

    /// <inheritdoc cref="MapObj"/>
    public interface IMapObj
    {
        /// <inheritdoc cref="MapObj.MapImageURL"/>
        string MapImageURL { get; set; }

        /// <inheritdoc cref="MapObj.ChapterPostURL"/>
        string ChapterPostURL { get; set; }

        /// <inheritdoc cref="MapObj.MapHeightInTiles"/>
        int MapHeightInTiles { get; }

        /// <inheritdoc cref="MapObj.MapWidthInTiles"/>
        int MapWidthInTiles { get; }

        /// <inheritdoc cref="MapObj.Constants"/>
        MapConstantsConfig Constants { get; set; }

        /// <inheritdoc cref="MapObj.Tiles"/>
        List<List<ITile>> Tiles { get; set; }

        /// <inheritdoc cref="MapObj.TileObjectInstances"/>
        Dictionary<int, ITileObjectInstance> TileObjectInstances { get; set; }

        /// <inheritdoc cref="MapObj.GetTileByCoord(ICoordinate)"/>
        ITile GetTileByCoord(ICoordinate coord);

        /// <inheritdoc cref="MapObj.GetTileByCoord(int, int)"/>
        ITile GetTileByCoord(int x, int y);

        /// <inheritdoc cref="MapObj.GetTilesInRadius(List{ITile}, int)"/>
        List<ITile> GetTilesInRadius(List<ITile> centerTiles, int radius);

        /// <inheritdoc cref="MapObj.GetTilesInRadius(ITile, int)"/>
        List<ITile> GetTilesInRadius(ITile centerTile, int radius);

        /// <inheritdoc cref="MapObj.GetTilesInRadius(ICoordinate, int)"/>
        List<ITile> GetTilesInRadius(ICoordinate center, int radius);
    }

    #endregion Interface

    /// <summary>
    /// Object representing the map.
    /// </summary>
    public class MapObj : IMapObj
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
        public List<List<ITile>> Tiles { get; set; }

        /// <summary>
        /// Dictionary of tile object instances present on the map.
        /// </summary>
        public Dictionary<int, ITileObjectInstance> TileObjectInstances { get; set; }

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
        public MapObj(MapConfig config, IDictionary<string, ITerrainType> terrainTypes, IDictionary<string, ITileObject> tileObjects)
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
            this.Tiles = new List<List<ITile>>();
            BuildTiles(config.MapTiles, terrainTypes);

            //If we have tile objects configured, add those to the map
            #warning Clean this up after old Tile Object config is no longer needed.
            this.TileObjectInstances = new Dictionary<int, ITileObjectInstance>();
            if (config.MapObjects != null)
            {
                //If we're using the new config, use the new config path. Else, fall back on the old config path.
                if(config.MapObjects.Name > -1)
                    AddTileObjectsToTiles(config.MapObjects, tileObjects);
                else
                    AddTileObjectsToTiles_Old(config.MapObjects, tileObjects);
            }
        }

        #endregion

        /// <summary>
        /// Calculates the expected height/width of the map in # of tiles based on the dimensions the image loaded from <c>MapImageURL</c>.
        /// </summary>
        private void GetMapDimensionsFromImage()
        {
            int tileHeight;
            int tileWidth;


            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    Task<byte[]> imageData = httpClient.GetByteArrayAsync(this.MapImageURL);
                    imageData.Wait();

                    using (MemoryStream imgStream = new MemoryStream(imageData.Result))
                    using (SKManagedStream inputStream = new SKManagedStream(imgStream))
                    using (SKBitmap img = SKBitmap.Decode(inputStream))
                    {
                        this.ImageHeight = img.Height;
                        this.ImageWidth = img.Width;
                    }
                }
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("404"))
                    throw new MapImageLoadFailedException();
                throw new MapImageLoadFailedException(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
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
        private void BuildTiles(MapTilesConfig config, IDictionary<string, ITerrainType> terrainTypes)
        {
            int x = 1;
            int y = 1;

            try
            {
                IList<IList<object>> tileData = config.Query.Data;
                IDictionary<int, List<ITile>> warpGroups = new Dictionary<int, List<ITile>>();

                if (tileData.Count != this.MapHeightInTiles)
                    throw new UnexpectedMapHeightException(tileData.Count, this.MapHeightInTiles, config.Query.Sheet);

                foreach (List<object> row in tileData)
                {
                    if (row.Count != this.MapWidthInTiles)
                        throw new UnexpectedMapWidthException(row.Count, this.MapWidthInTiles, config.Query.Sheet);

                    List<ITile> currentRow = new List<ITile>();
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
                        ICoordinate coord = new Coordinate(this.Constants.CoordinateFormat, x, y);
                        ITerrainType type = TerrainType.MatchName(terrainTypes, t, coord);
                        ITile temp = new Tile(coord, type);

                        //If we found a warp group number, add the new tile to a warp group.
                        if (warpGroupNum > 0)
                        {
                            //If the terrain type isn't configured as a warp, error.
                            if (type.WarpType == WarpType.None)
                                throw new TerrainTypeNotConfiguredAsWarpException(t, tile.ToString());

                            List<ITile> warpGroup;
                            if (!warpGroups.TryGetValue(warpGroupNum, out warpGroup))
                            {
                                warpGroup = new List<ITile>();
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
                    List<ITile> group = warpGroups[key];

                    IEnumerable<ITile> entrances = group.Where(w => w.TerrainType.WarpType == WarpType.Entrance || w.TerrainType.WarpType == WarpType.Dual);
                    IEnumerable<ITile> exits = group.Where(w => w.TerrainType.WarpType == WarpType.Exit || w.TerrainType.WarpType == WarpType.Dual);

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
        /// Uses the data from <paramref name="config"/> to build a dictionary of <c>TileObjectInstance</c>s and place them on the map.
        /// </summary>
        private void AddTileObjectsToTiles(MapObjectsConfig config, IDictionary<string, ITileObject> tileObjects)
        {
            this.TileObjectInstances = TileObjectInstance.BuildDictionary(config, this.Constants, tileObjects);
            foreach (ITileObjectInstance tileObjInst in this.TileObjectInstances.Values)
            {
                ITile originTile;

                try
                {
                    originTile = GetTileByCoord(tileObjInst.AnchorCoordinateObj);
                }
                catch (Exception ex)
                {
                    throw new TileObjectInstanceProcessingException(tileObjInst.TileObject.Name, ex);
                }

                try
                {
                    BindTileObjectToTiles(tileObjInst, originTile);
                }
                catch(Exception ex) 
                {
                    throw new MapProcessingException(ex);
                }
            }
        }

        /// <summary>
        /// Uses the data from <paramref name="config"/>'s query to apply <c>ITileObject</c>s to the map.
        /// </summary>
        [Obsolete("Leaving this function in until all teams using the old Tile Object placement method are finished.")]
        private void AddTileObjectsToTiles_Old(MapObjectsConfig config, IDictionary<string, ITileObject> tileObjects)
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
                    IList<ITile> tiles = this.Tiles[r];

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

                            ITileObject tileObj = TileObject.MatchName(tileObjects, value.Trim(), tiles[c].Coordinate);
                            TileObjectInstance tileObjInst = new TileObjectInstance(idIterator++, tileObj);

                            this.TileObjectInstances.Add(tileObjInst.ID, tileObjInst);
                            BindTileObjectToTiles(tileObjInst, tiles[c]);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new MapProcessingException(ex);
            }
        }

        private void BindTileObjectToTiles(ITileObjectInstance tileObjInst, ITile originTile)
        {
            //2-way bind the anchor tile
            originTile.TileObjects.Add(tileObjInst);
            tileObjInst.OriginTiles.Add(originTile);

            //2-way bind all origin tiles for multi-tile effects
            if (tileObjInst.TileObject.Size > 1)
            {
                int r = originTile.Coordinate.Y - 1; //row
                int c = originTile.Coordinate.X - 1; //column
                for (int r2 = r; r2 < r + tileObjInst.TileObject.Size; r2++)
                {
                    for (int c2 = c; c2 < c + tileObjInst.TileObject.Size; c2++)
                    {
                        //Skip the starting tile
                        if (r2 == r && c2 == c)
                            continue;

                        if (r2 >= this.Tiles.Count)
                            throw new TileOutOfBoundsException(new Coordinate(this.Constants.CoordinateFormat, r2, c2));

                        List<ITile> tiles2 = this.Tiles[r2];

                        if (c2 >= tiles2.Count)
                            throw new TileOutOfBoundsException(new Coordinate(this.Constants.CoordinateFormat, r2, c2));

                        //2-way bind
                        tiles2[c2].TileObjects.Add(tileObjInst);
                        tileObjInst.OriginTiles.Add(tiles2[c2]);
                    }
                }
            }
        }


        #region Tile Functions

        /// <summary>
        /// Fetches the tile with matching coordinates to <paramref name="coord"/>.
        /// </summary>
        /// <exception cref="TileOutOfBoundsException"></exception>
        public ITile GetTileByCoord(ICoordinate coord)
        {
            return GetTileByCoord(coord.X, coord.Y);
        }

        /// <summary>
        /// Fetches the tile with matching coordinates to <paramref name="x"/>,<paramref name="y"/>.
        /// </summary>
        /// <exception cref="TileOutOfBoundsException"></exception>
        public ITile GetTileByCoord(int x, int y)
        {
            List<ITile> row = this.Tiles.ElementAtOrDefault<List<ITile>>(y - 1) ?? throw new TileOutOfBoundsException(new Coordinate(this.Constants.CoordinateFormat, x, y));
            ITile column = row.ElementAtOrDefault<ITile>(x - 1) ?? throw new TileOutOfBoundsException(new Coordinate(this.Constants.CoordinateFormat, x, y));

            return column;
        }

        /// <summary>
        /// Returns a list of distinct tiles that are within <paramref name="radius"/> tiles of the <paramref name="centerTiles"/>.
        /// </summary>
        public List<ITile> GetTilesInRadius(List<ITile> centerTiles, int radius)
        {
            return centerTiles.SelectMany(t => GetTilesInRadius(t.Coordinate, radius)).Except(centerTiles).ToList();
        }

        /// <summary>
        /// Returns a list of distinct tiles that are within <paramref name="radius"/> tiles of the <paramref name="centerTile"/>.
        /// </summary>
        public List<ITile> GetTilesInRadius(ITile centerTile, int radius)
        {
            return GetTilesInRadius(centerTile.Coordinate, radius);
        }

        /// <summary>
        /// Returns a list of distinct tiles that are within <paramref name="radius"/> tiles of the <paramref name="center"/>.
        /// </summary>
        public List<ITile> GetTilesInRadius(ICoordinate center, int radius)
        {
            List<ITile> temp = new List<ITile>();

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
