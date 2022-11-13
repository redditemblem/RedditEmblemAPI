using Newtonsoft.Json;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Map
{
    /// <summary>
    /// Object representing a <c>TileObject</c> instance located on a <c>Tile</c>.
    /// </summary>
    public class TileObjectInstance
    {
        /// <summary>
        /// Unique ID that identifies this tile object instance.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Only for JSON serialization. The name of the tile object.
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.TileObject.Name; } }

        /// <summary>
        /// The <c>TileObject</c> located on the tile. 
        /// </summary>
        [JsonIgnore]
        public TileObject TileObject { get; set; }

        /// <summary>
        /// List of <c>Tile</c>s from which the tile object's range originates.
        /// </summary>
        [JsonIgnore]
        public List<Tile> OriginTiles { get; set; }

        /// <summary>
        /// Coordinate of the anchor tile. Assumes this is the always first tile in the OriginTiles list.
        /// </summary>
        [JsonProperty]
        private string AnchorCoordinate { get { return this.OriginTiles.First()?.Coordinate.AsText; } }

        /// <summary>
        /// List of the coordinates this tile object is capable of attacking.
        /// </summary>
        public List<Coordinate> AttackRange { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TileObjectInstance(int tileObjectID, TileObject tileObject)
        {
            this.ID = tileObjectID;
            this.TileObject = tileObject;
            this.OriginTiles = new List<Tile>();
            this.AttackRange = new List<Coordinate>();
        }
    }
}
