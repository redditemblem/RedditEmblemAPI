using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Items
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Engravings"</c> object data.
    /// </summary>
    public class EngravingsConfig : MultiQueryable
    {
        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of an engraving's sprite image URL value.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. List of stat modifiers to apply to engraved items.
        /// </summary>
        public List<NamedStatConfig> ItemStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. Container for item range override values.
        /// </summary>
        public ItemRangeConfig ItemRangeOverrides { get; set; } = new ItemRangeConfig() { Minimum = -1, Maximum = -1 };

        /// <summary>
        /// Optional. List of combat stat modifiers to apply to units.
        /// </summary>
        public List<NamedStatConfig> CombatStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of stat modifiers to apply to units.
        /// </summary>
        public List<NamedStatConfig> StatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of cell indexes for an engraving's tag(s).
        /// </summary>
        public List<int> Tags { get; set; } = new List<int>();

        /// <summary>
        /// Optional. List of cell indexes for an engraving's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
