using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Query;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace RedditEmblemAPI.Models.Output
{
    /// <summary>
    /// Object representing the map.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Initializes all of the <c>Map</c>'s attributes and builds its <c>Tiles</c> using the values in <paramref name="tileData"/>.
        /// </summary>
        /// <param name="mapImageURL"></param>
        /// <param name="chapterPostURL"></param>
        /// <param name="config"></param>
        /// <param name="tileData"></param>
        /// <exception cref="MapProcessingException"></exception>
        public Map(MapConfig config, IDictionary<string, TerrainType> terrainTypes, IDictionary<string, TerrainEffect> terrainEffects)
        {
            this.Constants = config.Constants;

            IList<object> values = config.MapControls.Query.Data.First();

            if ((values.ElementAtOrDefault(config.MapControls.MapSwitch) ?? "Off").ToString() != "On")
                throw new MapDataLockedException();

            string mapImageURL = (values.ElementAtOrDefault(config.MapControls.MapImageURL) ?? string.Empty).ToString();
            if (string.IsNullOrEmpty(mapImageURL))
                throw new MapImageURLNotFoundException(config.MapControls.Query.Sheet);
            this.MapImageURL = mapImageURL;

            //Build map
            this.Tiles = new List<List<Tile>>();
            BuildTiles(config.MapTiles.Query.Data, terrainTypes);

            if(config.MapEffects != null)
                ApplyTerrainEffects(config.MapEffects.Query.Data, terrainEffects);
        }

        /// <summary>
        /// The map's image URL.
        /// </summary>
        public string MapImageURL { get; set; }

        /// <summary>
        /// The height of the map image in pixels.
        /// </summary>
        public int MapImageHeight { get; private set; }

        /// <summary>
        /// The width of the map image in pixels.
        /// </summary>
        public int MapImageWidth { get; private set; }

        /// <summary>
        /// Collection of constant values for doing calculations.
        /// </summary>
        public MapConstantsConfig Constants { get; set; }

        /// <summary>
        /// Matrix of map tiles.
        /// </summary>
        public List<List<Tile>> Tiles { get; set; }

        /// <summary>
        /// Uses the raw <paramref name="tileData"/> matrix to contruct the <c>Tiles</c> matrix for this map.
        /// </summary>
        /// <param name="tileData"></param>
        /// <exception cref="MapProcessingException"></exception>
        private void BuildTiles(IList<IList<object>> tileData, IDictionary<string, TerrainType> terrainTypes)
        {
            int x = 1;
            int y = 1;

            int mapHeight;
            int mapWidth;

            try
            {
                GetMapDimensions(out mapHeight, out mapWidth);

                if (tileData.Count != mapHeight)
                    throw new UnexpectedMapHeightException(tileData.Count, mapHeight);

                foreach (IList<object> row in tileData)
                {
                    if (row.Count != mapWidth)
                        throw new UnexpectedMapWidthException(row.Count, mapWidth);

                    List<Tile> currentRow = new List<Tile>();
                    foreach (object tile in row)
                    {
                        //Match on tile's terrain type
                        TerrainType type;
                        if (!terrainTypes.TryGetValue(tile.ToString(), out type))
                            throw new UnmatchedTileTerrainException(x, y, tile.ToString());

                        type.Matched = true;
                        currentRow.Add(new Tile(x, y, type));
                        x++;
                    }

                    x = 1;
                    y++;
                    this.Tiles.Add(currentRow);
                }
            }
            catch(Exception ex)
            {
                throw new MapProcessingException(ex);
            }
            
        }

        /// <summary>
        /// Calculates the expected height and width of the map in tiles based on image size.
        /// </summary>
        /// <param name="tileHeight">The height of the map in # of tiles.</param>
        /// <param name="tileWidth">The width of the map in # of tiles.</param>
        private void GetMapDimensions(out int tileHeight, out int tileWidth)
        {
   
            byte[] imageData = new WebClient().DownloadData(this.MapImageURL);
            using (MemoryStream imgStream = new MemoryStream(imageData))
            using (SKManagedStream inputStream = new SKManagedStream(imgStream))
            using (SKBitmap img = SKBitmap.Decode(inputStream))
            {
                this.MapImageHeight = img.Height;
                this.MapImageWidth = img.Width;
            }

            tileHeight = (int)Math.Floor((decimal)this.MapImageHeight / (this.Constants.TileSize + this.Constants.TileSpacing));
            tileWidth = (int)Math.Floor((decimal)this.MapImageWidth / (this.Constants.TileSize + this.Constants.TileSpacing));

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
        }

        private void ApplyTerrainEffects(IList<IList<object>> tileData, IDictionary<string, TerrainEffect> terrainEffects)
        {
            try
            {
                if (tileData.Count > this.Tiles.Count)
                    throw new UnexpectedMapHeightException(tileData.Count, this.Tiles.Count);

                for (int r = 0; r < tileData.Count; r++)
                {
                    IList<object> row = tileData[r];
                    IList<Tile> tiles = this.Tiles[r];

                    if (row.Count > tiles.Count)
                        throw new UnexpectedMapWidthException(row.Count, tiles.Count);

                    for (int t = 0; t < row.Count; t++)
                    {
                        string values = row[t].ToString();
                        if (string.IsNullOrEmpty(values))
                            continue;
                        
                        foreach(string value in values.Split(","))
                        {
                            TerrainEffect effect;
                            if (!terrainEffects.TryGetValue(value.Trim(), out effect))
                                throw new UnmatchedTileEffectException(tiles[t].Coordinate, value.Trim());

                            effect.Matched = true;
                            tiles[t].TerrainEffectsList.Add(effect);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new MapProcessingException(ex);
            }
        }
    }
}
