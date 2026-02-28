using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;

namespace RedditEmblemAPI.Models.Configuration.Map
{
    /// <summary>
    /// Container class for deserialized JSON <c>"MapObjects"</c> object data.
    /// </summary>
    public class MapObjectsConfig : Queryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index of the name of a tile object placed on the map.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required. Cell index of a tile object's coordinate value.
        /// </summary>
        [JsonRequired]
        public int Coordinate { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Container object for configuration about a tile object's repeater options.
        /// </summary>
        public MapObjectRepeaterToolConfig RepeaterTool { get; set; } = null;

        /// <summary>
        /// Optional. Container object for configuration about a tile object's HP.
        /// </summary>
        public HPConfig HP { get; set; } = null;

        #endregion Optional Fields
    }
}
