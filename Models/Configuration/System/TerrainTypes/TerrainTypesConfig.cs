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
        public Query Query { get; set; }

        /// <summary>
        /// Cell index for the name of the terrain type.
        /// </summary>
        [JsonRequired]
        public int TypeName { get; set; }

        #endregion
    }
}