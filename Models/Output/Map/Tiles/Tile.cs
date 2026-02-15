using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Output.System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Map.Tiles
{
    #region Interface

    /// <inheritdoc cref="Tile"/>
    public interface ITile
    {
        /// <inheritdoc cref="Tile.Coordinate"/>
        ICoordinate Coordinate { get; set; }

        /// <inheritdoc cref="Tile.TerrainTypeObj"/>
        ITerrainType TerrainTypeObj { get; set; }

        /// <inheritdoc cref="Tile.UnitData"/>
        ITileUnitData UnitData { get; set; }

        /// <inheritdoc cref="Tile.WarpData"/>
        ITileWarpData WarpData { get; set; }

        /// <inheritdoc cref="Tile.TileObjects"/>
        List<ITileObjectInstance> TileObjects { get; set; }
    }

    #endregion Interface

    /// <summary>
    /// Object representing a single tile on the <c>Map</c>.
    /// </summary>
    public class Tile : ITile
    {
        #region Attributes

        /// <summary>
        /// The tile's horizontal/vertical location on the map.
        /// </summary>
        public ICoordinate Coordinate { get; set; }

        /// <summary>
        /// The terrain type of this tile.
        /// </summary>
        [JsonIgnore]
        public ITerrainType TerrainTypeObj { get; set; }

        /// <summary>
        /// Container for information about this tile's unit properties.
        /// </summary>
        public ITileUnitData UnitData { get; set; }

        /// <summary>
        /// Container for information about this tile's warp properties.
        /// </summary>
        public ITileWarpData WarpData { get; set; }

        /// <summary>
        /// List of the tile objects on this tile.
        /// </summary>
        [JsonIgnore]
        public List<ITileObjectInstance> TileObjects { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// Only for JSON serialization. Returns the name of the <c>TerrainType</c> of this tile.
        /// </summary>
        [JsonProperty]
        private string TerrainType { get { return this.TerrainTypeObj.Name; } }

        /// <summary>
        /// Only for JSON serialization. Returns List of the IDs of tile object instances on this tile.
        /// </summary>
        [JsonProperty]
        private IEnumerable<int> TileObjectInstanceIDs { get { return this.TileObjects.Select(to => to.ID); } }

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

        /// <summary>
        /// Only for JSON serialization. The number of tile objects with displayed ranges on this tile.
        /// </summary>
        [JsonProperty]
        private int TileObjCount { get { return 0; } }

        #endregion JSON Serialization Only

        #endregion Attributes

        #region Constructors

        /// <summary>
        /// Constructor. Initializes the tile's coordinate with the passed in <paramref name="x"/> and <paramref name="y"/> values.
        /// </summary>
        /// <param name="x">Used to initialize the Tile's coordinate in combination with <paramref name="y"/>.</param>
        /// <param name="y">Used to initialize the Tile's coordinate in combination with <paramref name="x"/>.</param>
        public Tile(CoordinateFormat coordinateFormat, int x, int y, ITerrainType terrainType)
            : this(new Coordinate(coordinateFormat, x, y), terrainType)
        { }

        /// <summary>
        /// Constructor. Initializes the tile's coordinate using <paramref name="coord"/>.
        /// </summary>
        public Tile(ICoordinate coord, ITerrainType terrainType)
        {
            this.Coordinate = coord;
            this.TerrainTypeObj = terrainType;

            this.UnitData = new TileUnitData();
            this.WarpData = new TileWarpData();
            this.TileObjects = new List<ITileObjectInstance>();
        }

        #endregion
    }
}
