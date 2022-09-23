using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Map
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Map"</c> object data.
    /// </summary>
    public class MapConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Container object for constant map values.
        /// </summary>
        [JsonRequired]
        public MapConstantsConfig Constants { get; set; }

        /// <summary>
        /// Required. Container object for map controls configuration.
        /// </summary>
        [JsonRequired]
        public MapControlsConfig MapControls { get; set; }

        /// <summary>
        /// Required. Container object for tiles configuration.
        /// </summary>
        [JsonRequired]
        public MapTilesConfig MapTiles { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Container object for map tile objects configuration.
        /// </summary>
        public MapObjectsConfig MapObjects { get; set; } = null;

        #endregion
    }
}