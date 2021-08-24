using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Map
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Constants"</c> object data.
    /// </summary>
    public class MapConstantsConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Height/width of each map tile measured in pixels.
        /// </summary>
        [JsonRequired]
        public int TileSize { get; set; }

        /// <summary>
        /// Required. Spacing between each map tile measured in pixels. 
        /// </summary>
        [JsonRequired]
        public int TileSpacing { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Flag signifying the existence of a header on the map image.
        /// </summary>
        public bool HasHeaderTopLeft { get; set; } = false;

        /// <summary>
        /// Optional. Flag signifying the existence of a footer on the map image.
        /// </summary>
        public bool HasHeaderBottomRight { get; set; } = false;

        /// <summary>
        /// Optional. Flag signifying whether units in the back of pair-ups should have their ranges calculated.
        /// </summary>
        public bool CalculatePairedUnitRanges { get; set; } = true;

        /// <summary>
        /// Optional. The name of the Unit Stat to use as the movement stat when calculating ranges. Defaults to "Mov".
        /// </summary>
        public string UnitMovementStatName { get; set; } = "Mov";

        #endregion
    }
}
