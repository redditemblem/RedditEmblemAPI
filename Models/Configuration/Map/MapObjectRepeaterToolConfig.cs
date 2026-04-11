using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Map
{
    public class MapObjectRepeaterToolConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a tile object repeater's shape.
        /// </summary>
        [JsonRequired]
        public (int, int) Shape { get; set; }

        /// <summary>
        /// Required. Location of a tile object repeater's height.
        /// </summary>
        [JsonRequired]
        public (int, int) Height { get; set; }

        /// <summary>
        /// Required. Location of a tile object repeater's width.
        /// </summary>
        [JsonRequired]
        public (int, int) Width { get; set; }
        
        #endregion Required Fields
    }
}
