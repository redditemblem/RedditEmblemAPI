using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Emblems
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Emblems"</c> object data.
    /// </summary>
    public class EmblemsConfig : MultiQueryable
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of an emblem's sprite image URL value.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of an emblem's tagline.
        /// </summary>
        public int Tagline { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of an emblem's engaged unit aura hex.
        /// </summary>
        public int EngagedUnitAura { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for an emblem's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion Optional Fields
    }
}
