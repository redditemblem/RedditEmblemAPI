using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Common
{
    /// <summary>
    /// Container class for deserialized JSON object data representing a named stat with a fixed value.
    /// </summary>
    public class NamedStatConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Name of the stat value. (ex. "Str")
        /// </summary>
        [JsonRequired]
        public string SourceName { get; set; }

        /// <summary>
        /// Required. Cell index for the stat's value.
        /// </summary>
        [JsonRequired]
        public int Value { get; set; }

        #endregion
    }
}