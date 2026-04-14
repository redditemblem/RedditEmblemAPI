using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.BattleStyles
{
    /// <summary>
    /// Container class for deserialized JSON <c>"BattleStyles"</c> object data.
    /// </summary>
    public class BattleStylesConfig : MultiQueryable
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Location of a battle style's sprite.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of a battle style's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
