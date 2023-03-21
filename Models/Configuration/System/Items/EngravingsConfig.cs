using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Items
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Engravings"</c> object data.
    /// </summary>
    public class EngravingsConfig : IQueryable
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of an engraving's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of an engraving's sprite image URL value.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. List of stat modifiers to apply to engraved items.
        /// </summary>
        public List<NamedStatConfig> StatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of cell indexes for an engraving's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
