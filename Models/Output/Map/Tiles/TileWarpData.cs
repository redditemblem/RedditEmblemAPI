using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Map.Tiles
{
    /// <summary>
    /// Container object for storing data about a tile's warp properties.
    /// </summary>
    public class TileWarpData
    {
        #region Attributes

        /// <summary>
        /// List of warp tiles this tile is linked to.
        /// </summary>
        [JsonIgnore]
        public List<Tile> WarpGroup { get; set; }

        /// <summary>
        /// Number associated with the warp group this tile is linked to.
        /// </summary>
        public int WarpGroupNumber { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// Only for JSON serialization. Flag indicating if this tile has been placed in a warp group.
        /// </summary>
        [JsonProperty]
        private bool InWarpGroup { get { return this.WarpGroup.Count > 0; } }

        /// <summary>
        /// Only for JSON serialization. List of warp tile coordinates that this tile is linked to.
        /// </summary>
        [JsonProperty]
        private string WarpGroupCoordinates { get { return string.Join("; ", this.WarpGroup.OrderBy(t => t.Coordinate.X).ThenBy(t => t.Coordinate.Y).Select(t => t.Coordinate.ToString())); } }


        #endregion JSON Serialization Only

        #endregion Attributes

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TileWarpData()
        {
            this.WarpGroup = new List<Tile>();
        }
    }
}
