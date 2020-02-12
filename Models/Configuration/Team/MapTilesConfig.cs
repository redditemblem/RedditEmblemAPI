using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.Team
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Tiles"</c> object data.
    /// </summary>
    public class MapTilesConfig
    {
        #region RequiredValues

        [JsonRequired]
        public Query Query { get; set; }

        #endregion
    }
}