using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.System.TerrainTypes
{
    /// <summary>
    /// Container class for deserialized JSON <c>"TerrainTypes"</c> object data.
    /// </summary>
    public class TerrainTypesConfig
    {
        #region RequiredFields

        [JsonRequired]
        public WorksheetQuery WorksheetQuery { get; set; }

        #endregion
    }
}