using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.Map
{
    /// <summary>
    /// Container class for deserialized JSON <c>"MapTiles"</c> object data.
    /// </summary>
    public class MapTilesConfig : IQueryable
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        #endregion
    }
}