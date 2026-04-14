using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.TileObjects
{
    public class TileObjectRangeConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a tile object's minimum range value.
        /// </summary>
        [JsonRequired]
        public (int, int) Minimum { get; set; }

        /// <summary>
        /// Required. Location of a tile object's maximum range value.
        /// </summary>
        [JsonRequired]
        public (int, int) Maximum { get; set; }

        #endregion Required Fields
    }
}
