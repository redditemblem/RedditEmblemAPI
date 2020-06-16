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
        /// Required. The icon for the currency's denomination. (Ex. $, G)
        /// </summary>
        [JsonRequired]
        public string CurrencySymbol { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Flag indicating if the <c>CurrencySymbol</c> appears on the left of numerical values.
        /// </summary>
        public bool IsSymbolLeftAligned { get; set; } = true;

        /// <summary>
        /// Optional. Flag indicating if there should be a space between the <c>CurrencySymbol</c> and numerical values.
        /// </summary>
        public bool IncludeSpace { get; set; } = false;

        #endregion
    }
}
