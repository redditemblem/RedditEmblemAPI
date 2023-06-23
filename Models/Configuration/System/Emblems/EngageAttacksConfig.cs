using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Emblems
{
    /// <summary>
    /// Container class for deserialized JSON <c>"EngageAttacks"</c> object data.
    /// </summary>
    public class EngageAttacksConfig : IQueryable
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of an engage attack's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index for the engage attack's sprite URL.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for an engage attack's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion Optional Fields
    }
}