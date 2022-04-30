using Newtonsoft.Json;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Map.Tiles
{
    /// <summary>
    /// Container object for storing data about a tile's unit properties.
    /// </summary>
    public class TileUnitData
    {
        #region Attributes 

        /// <summary>
        /// The unit occupying this tile, if any.
        /// </summary>
        [JsonIgnore]
        public Unit Unit { get; set; }

        /// <summary>
        /// Flag indicating if the <c>Unit</c> occupying this tile is anchored here. Units will be drawn at the anchor tile.
        /// </summary>
        public bool IsUnitAnchor { get; set; }

        /// <summary>
        /// Flag indicating if the <c>Unit</c> occupying this tile originates here. Units will have their range calculated from their origin tiles.
        /// </summary>
        [JsonIgnore]
        public bool IsUnitOrigin { get; set; }

        /// <summary>
        /// The nearby <c>Unit</c>s obstructing movement through this tile, if any.
        /// </summary>
        [JsonIgnore]
        public List<Unit> UnitsObstructingMovement { get; set; }

        /// <summary>
        /// The nearby <c>Unit</c>s obstructing item ranges through this tile, if any.
        /// </summary>
        [JsonIgnore]
        public List<Unit> UnitsObstructingItems { get; set; }

        /// <summary>
        /// The nearby <c>Unit</c>s adjusting the movement costs of this tile, if any.
        /// </summary>
        [JsonIgnore]
        public List<Unit> UnitsAffectingMovementCosts { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// Only for JSON serialization. Returns the name of the <c>Unit</c> on this tile. If <c>Unit</c> is null, returns an empty string.
        /// </summary>
        [JsonProperty]
        private string OccupyingUnitName { get { return (this.Unit == null ? string.Empty : this.Unit.Name); } }

        /// <summary>
        /// Only for JSON serialization. Returns the name of the paired <c>Unit</c> on this tile. If <c>Unit</c> is null or there is no paired unit, returns an empty string.
        /// </summary>
        [JsonProperty]
        private string PairedUnitName { get { return (this.Unit == null || this.Unit.Location.PairedUnitObj == null ? string.Empty : this.Unit.Location.PairedUnitObj.Name); } }

        #endregion JSON Serialization Only

        #endregion Attributes

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TileUnitData()
        {
            this.Unit = null;
            this.UnitsObstructingMovement = new List<Unit>();
            this.UnitsObstructingItems = new List<Unit>();
            this.UnitsAffectingMovementCosts = new List<Unit>();
        }
    }
}
