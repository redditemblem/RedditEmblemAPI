using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.Emblems
{
    /// <summary>
    /// Container class for deserialized JSON <c>"EngageAttacks"</c> object data.
    /// </summary>
    public class EngageAttacksConfig : MultiQueryable
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Location of the engage attack's sprite URL.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of an engage attack's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}