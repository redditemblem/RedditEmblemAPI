using Newtonsoft.Json;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Map.Tiles
{
    /// <summary>
    /// Object representing a single tile on the <c>Map</c>.
    /// </summary>
    public class Tile
    {
        #region Attributes

        /// <summary>
        /// The tile's horizontal/vertical location on the map.
        /// </summary>
        public Coordinate Coordinate { get; set; }

        /// <summary>
        /// The terrain type of this tile.
        /// </summary>
        [JsonIgnore]
        public TerrainType TerrainTypeObj { get; set; }

        /// <summary>
        /// Container for information about this tile's unit properties.
        /// </summary>
        public TileUnitData UnitData { get; set; }

        /// <summary>
        /// Container for information about this tile's warp properties.
        /// </summary>
        public TileWarpData WarpData { get; set; }

        /// <summary>
        /// List of the terrain effects on this tile.
        /// </summary>
        public IList<TileTerrainEffect> TerrainEffects { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// Only for JSON serialization. Returns the name of the <c>TerrainType</c> of this tile.
        /// </summary>
        [JsonProperty]
        private string TerrainType { get { return this.TerrainTypeObj.Name; } }

        /// <summary>
        /// Only for JSON serialization. The number of units with displayed movement on this tile.
        /// </summary>
        [JsonProperty]
        private int MovCount { get { return 0; } }

        /// <summary>
        /// Only for JSON serialization. The number of units with displayed attack range on this tile.
        /// </summary>
        [JsonProperty]
        private int AtkCount { get { return 0; } }

        /// <summary>
        /// Only for JSON serialization. The number of units with displayed utility range on this tile.
        /// </summary>
        [JsonProperty]
        private int UtilCount { get { return 0; } }

        #endregion JSON Serialization Only

        #endregion Attributes

        #region Constructors

        /// <summary>
        /// Initializes the tile's coordinate with the passed in <paramref name="x"/> and <paramref name="y"/> values.
        /// </summary>
        /// <param name="x">Used to initialize the Tile's coordinate in combination with <paramref name="y"/>.</param>
        /// <param name="y">Used to initialize the Tile's coordinate in combination with <paramref name="x"/>.</param>
        public Tile(int x, int y, TerrainType terrainType)
        {
            this.Coordinate = new Coordinate(x, y);
            this.TerrainTypeObj = terrainType;

            this.UnitData = new TileUnitData();
            this.WarpData = new TileWarpData();
            this.TerrainEffects = new List<TileTerrainEffect>();
        }

        #endregion
    }
}
