using Newtonsoft.Json;
using RedditEmblemAPI.Models.Common;

namespace RedditEmblemAPI.Models.Output
{
    /// <summary>
    /// Object representing a single Tile on the map.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Initializes the tile's coordinate with the passed in <paramref name="x"/> and <paramref name="y"/> values.
        /// </summary>
        /// <param name="x">Used to initialize the Tile's coordinate in combination with <paramref name="y"/>.</param>
        /// <param name="y">Used to initialize the Tile's coordinate in combination with <paramref name="x"/>.</param>
        public Tile(int x, int y)
        {
            this.Coordinate = new Coordinate(x, y);
        }

        /// <summary>
        /// The tile's location on the map.
        /// </summary>
        public Coordinate Coordinate { get; set; }

        /// <summary>
        /// The unit occupying this tile, if any.
        /// </summary>
        [JsonIgnore]
        public Unit Unit { get; set; }

        /// <summary>
        /// Returns the name of the <c>Unit</c> on this tile. If <c>Unit</c> is null, returns an empty string.
        /// </summary>
        public string OccupyingUnitName { get { return (this.Unit == null ? string.Empty : this.Unit.Name); } }

        /// <summary>
        /// The terrain type of this tile.
        /// </summary>
        [JsonIgnore]
        public TerrainType Terrain { get; set; }

        /// <summary>
        /// Returns the name of the <c>TerrainType</c> of this tile.
        /// </summary>
        //TO DO: REMOVE THE NULL CHECK ON THIS. IT SHOULD NEVER BE NULL.
        public string TerrainTypeName { get { return (this.Terrain == null ? string.Empty : this.Terrain.Name);  } }
    }
}
