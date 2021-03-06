﻿using Newtonsoft.Json;

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

        #endregion
    }
}
