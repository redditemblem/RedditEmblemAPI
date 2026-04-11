using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.Items
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Engravings"</c> object data.
    /// </summary>
    public class EngravingsConfig : MultiQueryable
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Location of an engraving's sprite image URL value.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of stat modifiers to apply to engraved items.
        /// </summary>
        public NamedStatConfig[] ItemStatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Container for item range override values.
        /// </summary>
        public ItemRangeConfig ItemRangeOverrides { get; set; } = new ItemRangeConfig() { Minimum = (-1, -1), Maximum = (-1, -1) };

        /// <summary>
        /// Optional. Collection of combat stat modifiers to apply to units.
        /// </summary>
        public NamedStatConfig[] CombatStatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of stat modifiers to apply to units.
        /// </summary>
        public NamedStatConfig[] StatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of locations of an engraving's tag(s).
        /// </summary>
        public (int, int)[] Tags { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Collection of locations of an engraving's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
