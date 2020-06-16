using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.Items
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Range"</c> object data.
    /// </summary>
    public class RangeConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index for an item's minimum range value.
        /// </summary>
        [JsonRequired]
        public int Minimum { get; set; }

        /// <summary>
        /// Required. Cell index for an item's minimum range value.
        /// </summary>
        [JsonRequired]
        public int Maximum { get; set; }

        #endregion
    }
}