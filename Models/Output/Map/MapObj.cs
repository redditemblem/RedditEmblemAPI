using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Query;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

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
        public int TileHeight { get; private set; }

        /// <summary>
        /// The width of the map in number of tiles.
        /// </summary>
        [JsonIgnore]
        public int TileWidth { get; private set; }

        /// <summary>
        /// Collection of constant values for doing calculations.
        /// </summary>
        public MapConstantsConfig Constants { get; set; }

        /// <summary>
        /// Matrix of map tiles.
        /// </summary>
        public List<List<Tile>> Tiles { get; set; }

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
        public MapObj(MapConfig config, IDictionary<string, TerrainType> terrainTypes, IDictionary<string, TerrainEffect> terrainEffects)
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
            this.MapImageURL = mapImageURL;

            this.ChapterPostURL = (values.ElementAtOrDefault(config.MapControls.ChapterPostURL) ?? string.Empty).ToString();

            GetMapDimensionsFromImage();

            //Build tile matrix
            this.Tiles = new List<List<Tile>>();
            BuildTiles(config.MapTiles, terrainTypes);

            //If we have terrain effects configured, add those to the map
            if (config.MapEffects != null)
                AddTerrainEffectsToTiles(config.MapEffects, terrainEffects);
        }

        #endregion

        /// <summary>
        /// Calculates the expected height/width of the map in # of tiles based on the dimensions the image loaded from <c>MapImageURL</c>.
        /// </summary>
        private void GetMapDimensionsFromImage()
        {
            int tileHeight;
            int tileWidth;

            byte[] imageData = new WebClient().DownloadData(this.MapImageURL);
            using (MemoryStream imgStream = new MemoryStream(imageData))
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

            this.TileHeight = tileHeight;
            this.TileWidth = tileWidth;
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
                IDictionary<int, IList<Tile>> warpGroups = new Dictionary<int, IList<Tile>>();

                if (tileData.Count != this.TileHeight)
                    throw new UnexpectedMapHeightException(tileData.Count, this.TileHeight, config.Query.Sheet);

                foreach (IList<object> row in tileData)
                {
                    if (row.Count != this.TileWidth)
                        throw new UnexpectedMapWidthException(row.Count, this.TileWidth, config.Query.Sheet);

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
                        TerrainType type;
                        if (!terrainTypes.TryGetValue(t, out type))
                            throw new UnmatchedTileTerrainTypeException(x, y, tile.ToString());

                        type.Matched = true;
                        Tile temp = new Tile(x, y, type);

                        //If we found a warp group number, add the new tile to a warp group.
                        if (warpGroupNum > 0)
                        {
                            //If the terrain type isn't configured as a warp, error.
                            if (type.WarpType == WarpType.None)
                                throw new TerrainTypeNotConfiguredAsWarpException(t, tile.ToString());

                            IList<Tile> warpGroup;
                            if (!warpGroups.TryGetValue(warpGroupNum, out warpGroup))
                            {
                                warpGroup = new List<Tile>();
                                warpGroups.Add(warpGroupNum, warpGroup);
                            }

                            //Two-way bind warp group data
                            warpGroup.Add(temp);
                            temp.WarpGroup = warpGroup;
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
                    IList<Tile> group = warpGroups[key];

                    IList<Tile> entrances = group.Where(w => w.TerrainTypeObj.WarpType == WarpType.Entrance || w.TerrainTypeObj.WarpType == WarpType.Dual).ToList();
                    IList<Tile> exits = group.Where(w => w.TerrainTypeObj.WarpType == WarpType.Exit || w.TerrainTypeObj.WarpType == WarpType.Dual).ToList();

                    //If we do not have at least one distinct entrance and exit
                    if (entrances.Count == 0
                        || exits.Count == 0
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
        /// Uses the data from <paramref name="config"/>'s query to apply <c>TerrainEffect</c>s to the map.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="terrainEffects"></param>
        private void AddTerrainEffectsToTiles(MapEffectsConfig config, IDictionary<string, TerrainEffect> terrainEffects)
        {
            try
            {
                IList<IList<object>> tileData = config.Query.Data;

                //Not every tile has to be mapped on this page, so just ensure we don't exceed the existing map size.
                if (tileData.Count > this.Tiles.Count)
                    throw new UnexpectedMapHeightException(tileData.Count, this.Tiles.Count, config.Query.Sheet);

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
                            if (string.IsNullOrEmpty(value))
                                continue;

                            TerrainEffect effect;
                            if (!terrainEffects.TryGetValue(value.Trim(), out effect))
                                throw new UnmatchedTileTerrainEffectException(tiles[c].Coordinate, value.Trim());
                            effect.Matched = true;
                            tiles[c].TerrainEffects.Add(new TileTerrainEffect(effect, true));

                            //Set all tiles for multi-tile effects
                            if (effect.Size > 1)
                            {
                                for (int r2 = r; r2 < r + effect.Size; r2++)
                                {
                                    for (int c2 = c; c2 < c + effect.Size; c2++)
                                    {
                                        //Skip the starting tile
                                        if (r2 == r && c2 == c)
                                            continue;

                                        if (r2 >= this.Tiles.Count)
                                            throw new TileOutOfBoundsException(r2, c2);

                                        IList<Tile> tiles2 = this.Tiles[r2];

                                        if (c2 >= tiles2.Count)
                                            throw new TileOutOfBoundsException(r2, c2);

                                        tiles2[c2].TerrainEffects.Add(new TileTerrainEffect(effect, false));
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
            IList<Tile> row = this.Tiles.ElementAtOrDefault<IList<Tile>>(y - 1) ?? throw new TileOutOfBoundsException(x, y);
            Tile column = row.ElementAtOrDefault<Tile>(x - 1) ?? throw new TileOutOfBoundsException(x, y);

            return column;
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
            IList<Tile> temp = new List<Tile>();

            for (int x = 0; x <= radius; x++)
            {
                for (int y = 0; y <= radius; y++)
                {
                    if (x == 0 && y == 0) continue; //ignore origin
                    if (x + y > radius) continue; //if the total displacement is greater than the radius, stop

                    //If fetching any tile fails, we still want to continue execution
                    try { temp.Add(GetTileByCoord(center.X + x, center.Y + y)); } catch (TileOutOfBoundsException ex) { }
                    try { temp.Add(GetTileByCoord(center.X - x, center.Y + y)); } catch (TileOutOfBoundsException ex) { }
                    try { temp.Add(GetTileByCoord(center.X + x, center.Y - y)); } catch (TileOutOfBoundsException ex) { }
                    try { temp.Add(GetTileByCoord(center.X - x, center.Y - y)); } catch (TileOutOfBoundsException ex) { }
                }
            }

            //Return distinct list of tiles
            return temp.Distinct().ToList();
        }

        #endregion
    }
}
