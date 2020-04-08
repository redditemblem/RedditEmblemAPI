using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Team
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Team"</c> object data.
    /// </summary>
    public class TeamConfig
    {
        #region RequiredFields

        /// <summary>
        /// The team's name as it should appear to a user in the UI. Must match the name of .json file in the JSON directory, spaces removed.
        /// </summary>
        [JsonRequired]
        public string Name { get; set; }

        /// <summary>
        /// The ID of the Google Sheets workbook from which to query all map data.
        /// </summary>
        [JsonRequired]
        public string WorkbookID { get; set; }

        /// <summary>
        /// Container object for map configuration.
        /// </summary>
        [JsonRequired]
        public MapConfig Map { get; set; }

        #endregion
    }
}