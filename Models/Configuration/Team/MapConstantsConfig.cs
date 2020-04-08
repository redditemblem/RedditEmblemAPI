using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Team
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Constants"</c> object data.
    /// </summary>
    public class MapConstantsConfig
    {
        #region RequiredValues

        /// <summary>
        /// Height/width of each map tile measured in pixels.
        /// </summary>
        [JsonRequired]
        public int TileSize { get; set; }

        /// <summary>
        /// Spacing between each map tile measured in pixels. 
        /// </summary>
        [JsonRequired]
        public int TileSpacing { get; set; }

        #endregion

        #region OptionalValues

        /// <summary>
        /// Flag signifying the existence of a header on the map image. When true, offsets the all tile rows right by <c>TileSize</c> pixels and omits the first row entirely.
        /// </summary>
        public bool HasHeaderTopLeft { get; set; } = false;

        /// <summary>
        /// Flag signifying the existence of a footer on the map image. When true, truncates all tile rows by <c>TileSize</c> pixels and omits the final row entirely.
        /// </summary>
        public bool HasHeaderBottomRight { get; set; } = false;

        #endregion
    }
}
