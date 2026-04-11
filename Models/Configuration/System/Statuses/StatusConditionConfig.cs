using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.Statuses
{
    /// <summary>
    /// Container class for deserialized JSON <c>"StatusConditions"</c> object data.
    /// </summary>
    public class StatusConditionConfig : MultiQueryable
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Location of a status condition's sprite.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a status condition's type value.
        /// </summary>
        public (int, int) Type { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a status condition's turns value.
        /// </summary>
        public (int, int) Turns { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of a status condition's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Collection of status condition effect configurations.
        /// </summary>
        public StatusConditionEffectConfig[] Effects { get; set; } = Array.Empty<StatusConditionEffectConfig>();

        #endregion Optional Fields
    }
}
