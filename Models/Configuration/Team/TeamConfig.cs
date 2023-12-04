using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Team
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Team"</c> object data.
    /// </summary>
    public class TeamConfig
    {
        #region Required Fields

        /// <summary>
        /// The team's name formatted as it should appear to a user in the UI. Must match the name of .json file in the JSON directory, spaces omitted.
        /// </summary>
        [JsonRequired]
        public string Name { get; set; }

        /// <summary>
        /// The ID of the Google Sheets workbook from which to query all data.
        /// </summary>
        [JsonRequired]
        public string WorkbookID { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. If provided, the "Google Sheets" link in the UI will redirect to this workbook ID instead of the main workbook ID. Intended for teams that use separate GM and player sheets.
        /// </summary>
        public string AlternativeWorkbookID { get; set; } = string.Empty;

        #endregion Optional Fields
    }
}