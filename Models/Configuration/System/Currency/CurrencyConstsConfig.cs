using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Currency"</c> object data.
    /// </summary>
    public class CurrencyConstsConfig
    {
        #region Required Fields

        /// <summary>
        /// The icon for the currency's denomination. (Ex. $, G)
        /// </summary>
        [JsonRequired]
        public string CurrencyIcon { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Flag indicating if the <c>CurrencyIcon</c> appears on the left of numerical values.
        /// </summary>
        public bool IsIconLeftAligned { get; set; } = true;

        /// <summary>
        /// Flag indicating if there should be a space between the <c>CurrencyIcon</c> and numerical values.
        /// </summary>
        public bool IncludeSpace { get; set; } = false;

        #endregion
    }
}
