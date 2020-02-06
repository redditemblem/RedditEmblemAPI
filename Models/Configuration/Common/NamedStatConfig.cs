using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Common
{
    /// <summary>
    /// Container class for deserialized JSON object data.
    /// </summary>
    public class NamedStatConfig
    {
        #region RequiredFields

        /// <summary>
        /// Name of the stat value. (ex. "Atk")
        /// </summary>
        [JsonRequired]
        public string SourceName { get; set; }

        /// <summary>
        /// Cell index for the stat's value.
        /// </summary>
        [JsonRequired]
        public int Value { get; set; }

        #endregion
    }
}