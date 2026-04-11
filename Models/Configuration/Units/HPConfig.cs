using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    /// <summary>
    /// Container class for deserialized JSON <c>"HP"</c> object data.
    /// </summary>
    public class HPConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a unit's current hit point value.
        /// </summary>
        [JsonRequired]
        public (int, int) Current { get; set; }

        /// <summary>
        /// Required. Location of a unit's maximum hit point value.
        /// </summary>
        [JsonRequired]
        public (int, int) Maximum { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of the unit's remaining number of hit point bars.
        /// </summary>
        public (int, int) RemainingBars { get; set; } = (-1, -1);

        #endregion Optional Fields
    }
}