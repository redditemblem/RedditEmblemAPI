using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.Emblems
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Emblems"</c> object data.
    /// </summary>
    public class EmblemsConfig : MultiQueryable
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Location of an emblem's sprite image URL value.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of an emblem's tagline.
        /// </summary>
        public (int, int) Tagline { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of an emblem's engaged unit aura hex.
        /// </summary>
        public (int, int) EngagedUnitAura { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of an emblem's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
