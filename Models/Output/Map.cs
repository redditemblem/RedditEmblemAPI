using RedditEmblemAPI.Models.Configuration.Team;
using RedditEmblemAPI.Models.Exceptions;
using System;
using System.Collections.Generic;

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
        public Map(string mapImageURL, string chapterPostURL, MapConstantsConfig config, IList<IList<object>> tileData, Dictionary<string, TerrainType> terrainTypes)
        {
            this.MapImageURL = mapImageURL;
            this.ChapterPostURL = chapterPostURL;
            this.Constants = config;
            this.Tiles = new List<List<Tile>>();

            BuildTiles(tileData, terrainTypes);
        }

        /// <summary>
        /// The map's image URL.
        /// </summary>
        public string MapImageURL { get; set; }

        /// <summary>
        /// The URL of the chapter post on Reddit.
        /// </summary>
        public string ChapterPostURL { get; set; }

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
        private void BuildTiles(IList<IList<object>> tileData, Dictionary<string, TerrainType> terrainTypes)
        {
            int x = 1;
            int y = 1;
            try
            {
                foreach (IList<object> row in tileData)
                {
                    List<Tile> currentRow = new List<Tile>();
                    foreach (object tile in row)
                    {
                        //Match on tile's terrain type
                        TerrainType type;
                        if (!terrainTypes.TryGetValue(tile.ToString(), out type))
                            throw new UnmatchedTileTerrainException(x, y, tile.ToString());

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
    }
}
