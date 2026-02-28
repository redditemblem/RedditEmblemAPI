using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Map
{
    public class MapSegmentConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index of the map segment's image URL value.
        /// </summary>
        [JsonRequired]
        public int ImageURL { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of the map segment's title.
        /// </summary>
        public int Title { get; set; } = -1;

        #endregion Optional Fields
    }
}
