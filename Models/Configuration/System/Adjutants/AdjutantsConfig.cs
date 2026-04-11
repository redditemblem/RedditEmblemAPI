using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.Adjutants
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Adjutants"</c> object data.
    /// </summary>
    public class AdjutantsConfig : MultiQueryable
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Location of an adjutant's sprite image URL value.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of modifiers that should be applied to the unit's combat stats when this adjutant is equipped.
        /// </summary>
        public NamedStatConfig[] CombatStatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of modifiers that should be applied to the unit's general stats when this adjutant is equipped.
        /// </summary>
        public NamedStatConfig[] StatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of locations for an adjutant's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
