using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.TileObjects
{
    public class TileObjectRangeConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index for a tile object's minimum range value.
        /// </summary>
        [JsonRequired]
        public int Minimum { get; set; }

        /// <summary>
        /// Required. Cell index for a tile object's maximum range value.
        /// </summary>
        [JsonRequired]
        public int Maximum { get; set; }

        #endregion
    }
}
