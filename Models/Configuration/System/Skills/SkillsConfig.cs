using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.Skills
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Skills"</c> object data.
    /// </summary>
    public class SkillsConfig : MultiQueryable
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Location of a skill's sprite image URL value.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of a skill's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Collection of skill effect configurations.
        /// </summary>
        public SkillEffectConfig[] Effects { get; set; } = Array.Empty<SkillEffectConfig>();

        #endregion Optional Fields
    }
}
