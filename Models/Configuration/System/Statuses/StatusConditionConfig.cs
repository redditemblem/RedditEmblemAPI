using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Statuses
{
    /// <summary>
    /// Container class for deserialized JSON <c>"StatusConditions"</c> object data.
    /// </summary>
    public class StatusConditionConfig
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index for a status condition's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index for a status condition's sprite.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index for a status condition's type value.
        /// </summary>
        public int Type { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index for a status condition's turns value.
        /// </summary>
        public int Turns { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for a status condition's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        /// <summary>
        /// Optional. Container object for status condition effect configuration.
        /// </summary>
        public StatusConditionEffectConfig Effect { get; set; } = null;

        #endregion
    }
}
