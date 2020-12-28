using Newtonsoft.Json;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Map
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
        /// The unit occupying this tile, if any.
        /// </summary>
        [JsonIgnore]
        public Unit Unit { get; set; }

        /// <summary>
        /// Only for JSON serialization. Returns the name of the <c>Unit</c> on this tile. If <c>Unit</c> is null, returns an empty string.
        /// </summary>
        [JsonProperty]
        private string OccupyingUnitName { get { return (this.Unit == null ? string.Empty : this.Unit.Name); } }

        /// <summary>
        /// Only for JSON serialization. Returns the name of the paired <c>Unit</c> on this tile. If <c>Unit</c> is null or there is no paired unit, returns an empty string.
        /// </summary>
        [JsonProperty]
        private string PairedUnitName { get { return (this.Unit == null || this.Unit.PairedUnitObj == null ? string.Empty : this.Unit.PairedUnitObj.Name); } }

        /// <summary>
        /// Flag indicating if the <c>Unit</c> occupying this tile is anchored here. Units will be drawn at the anchor tile.
        /// </summary>
        public bool IsUnitAnchor { get; set; }

        /// <summary>
        /// Flag indicating if the <c>Unit</c> occupying this tile originates here. Units will have their range calculated from the origin tile.
        /// </summary>
        public bool IsUnitOrigin { get; set; }

        /// <summary>
        /// The terrain type of this tile.
        /// </summary>
        [JsonIgnore]
        public TerrainType TerrainTypeObj { get; set; }

        /// <summary>
        /// Only for JSON serialization. Returns the name of the <c>TerrainType</c> of this tile.
        /// </summary>
        [JsonProperty]
        private string TerrainType { get { return this.TerrainTypeObj.Name;  } }

        /// <summary>
        /// List of warp tiles this tile is linked to.
        /// </summary>
        [JsonIgnore]
        public IList<Tile> WarpGroup { get; set; }

        /// <summary>
        /// List of the terrain effects on this tile.
        /// </summary>
        public IList<TileTerrainEffect> TerrainEffects { get; set; }

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

        #endregion

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
            this.TerrainEffects = new List<TileTerrainEffect>();
        }

        #endregion
    }
}
