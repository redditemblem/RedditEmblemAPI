using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.Map
{
    /// <summary>
    /// Container class for deserialized JSON <c>"MapControls"</c> object data.
    /// </summary>
    public class MapControlsConfig : Queryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index of the map switch value.
        /// </summary>
        [JsonRequired]
        public int MapSwitch { get; set; }

        /// <summary>
        /// Required. Cell index of the map image URL value.
        /// </summary>
        [JsonRequired]
        public int MapImageURL { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of chapter post URL value.
        /// </summary>
        public int ChapterPostURL { get; set; } = -1;

        #endregion
    }
}
