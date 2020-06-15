using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.Team
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Map"</c> object data.
    /// </summary>
    public class MapConfig
    {
        #region RequiredFields

        [JsonRequired]
        public Query Query { get; set; }
        
        /// <summary>
        /// Cell index of the map switch value.
        /// </summary>
        [JsonRequired]
        public int MapSwitch { get; set; }

        /// <summary>
        /// Cell index of the map image URL value.
        /// </summary>
        [JsonRequired]
        public int MapURL { get; set; }

        /// <summary>
        /// Container object for constant map values.
        /// </summary>
        [JsonRequired]
        public MapConstantsConfig Constants { get; set; }

        /// <summary>
        /// Container object for tiles configuration.
        /// </summary>
        [JsonRequired]
        public MapTilesConfig Tiles { get; set; }

        #endregion

        #region OptionalFields

        /// <summary>
        /// Cell index of chapter post URL value.
        /// </summary>
        public int ChapterPostURL { get; set; } = -1;

        /// <summary>
        /// Container object for effects configuration.
        /// </summary>
        public MapEffectsConfig Effects { get; set; } = null;

        #endregion
    }
}