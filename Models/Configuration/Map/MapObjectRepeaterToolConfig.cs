using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Map
{
    public class MapObjectRepeaterToolConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index for a tile object repeater's shape.
        /// </summary>
        [JsonRequired]
        public int Shape { get; set; }

        /// <summary>
        /// Required. Cell index for a tile object repeater's height.
        /// </summary>
        [JsonRequired]
        public int Height { get; set; }

        /// <summary>
        /// Required. Cell index for a tile object repeater's width.
        /// </summary>
        [JsonRequired]
        public int Width { get; set; }
        
        #endregion Required Fields
    }
}
