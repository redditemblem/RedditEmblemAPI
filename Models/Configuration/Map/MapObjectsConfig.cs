using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;

namespace RedditEmblemAPI.Models.Configuration.Map
{
    /// <summary>
    /// Container class for deserialized JSON <c>"MapObjects"</c> object data.
    /// </summary>
    public class MapObjectsConfig : IQueryable
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of the name of a tile object placed on the map.
        /// </summary>
        /// <remarks>Requirement not currently enforced as teams are still using the old config setup.</remarks>
        //[JsonRequired]
        public int Name { get; set; } = -1;

        /// <summary>
        /// Required. Cell index of a tile object's coordinate value.
        /// </summary>
        /// <remarks>Requirement not currently enforced as teams are still using the old config setup.</remarks>
        //[JsonRequired]
        public int Coordinate { get; set; } = -1;

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Container object for configuration about a tile object's HP.
        /// </summary>
        public HPConfig HP { get; set; } = null;

        #endregion Optional Fields
    }
}
