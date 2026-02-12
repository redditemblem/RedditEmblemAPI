using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Emblems
{
    /// <summary>
    /// Container class for deserialized JSON <c>"EngageAttacks"</c> object data.
    /// </summary>
    public class EngageAttacksConfig : MultiQueryable
    {
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