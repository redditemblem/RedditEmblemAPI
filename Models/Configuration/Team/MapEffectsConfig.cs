using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.Team
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Effects"</c> object data.
    /// </summary>
    public class MapEffectsConfig
    {
        #region RequiredValues

        [JsonRequired]
        public Query Query { get; set; }

        #endregion
    }
}
