using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.Items
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Range"</c> object data.
    /// </summary>
    public class RangeConfig
    {
        #region RequiredFields

        /// <summary>
        /// Cell index for an item's minimum range value.
        /// </summary>
        [JsonRequired]
        public int Minimum { get; set; }

        /// <summary>
        /// Cell index for an item's minimum range value.
        /// </summary>
        [JsonRequired]
        public int Maximum { get; set; }

        #endregion
    }
}