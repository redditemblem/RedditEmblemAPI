using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.Map
{
    /// <summary>
    /// Container class for deserialized JSON <c>"MapObjects"</c> object data.
    /// </summary>
    public class MapObjectsConfig
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        #endregion
    }
}
