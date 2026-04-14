using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.Items
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Items"</c> object data.
    /// </summary>
    public class ItemsConfig : MultiQueryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of an item's category value.
        /// </summary>
        [JsonRequired]
        public (int, int) Category { get; set; }

        /// <summary>
        /// Required. Collection of locations of an item's utilized stat value(s).
        /// </summary>
        [JsonRequired]
        public (int, int)[] UtilizedStats { get; set; }

        /// <summary>
        /// Required. Location of an item's deal damage flag.
        /// </summary>
        [JsonRequired]
        public (int, int) DealsDamage { get; set; }

        /// <summary>
        /// Required. Location of an item's uses value.
        /// </summary>
        [JsonRequired]
        public (int, int) Uses { get; set; }

        /// <summary>
        /// Required. Collection of an item's stat configurations.
        /// </summary>
        [JsonRequired]
        public NamedStatConfig_Displayed[] Stats { get; set; }

        /// <summary>
        /// Required. Container object for an item's range configuration.
        /// </summary>
        [JsonRequired]
        public ItemRangeConfig Range { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of an item's sprite image URL value.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of an item's weapon rank value.
        /// </summary>
        public (int, int) WeaponRank { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of an item's is always usable flag.
        /// </summary>
        public (int, int) IsAlwaysUsable { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of an item's targeted stats value.
        /// </summary>
        public (int, int)[] TargetedStats { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Collection of an item's combat stat modifiers when equipped.
        /// </summary>
        public NamedStatConfig[] EquippedCombatStatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of an item's stat modifiers when equipped.
        /// </summary>
        public NamedStatConfig[] EquippedStatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of an item's skills that activate when equipped.
        /// </summary>
        public UnitSkillConfig[] EquippedSkills { get; set; } = Array.Empty<UnitSkillConfig>();

        /// <summary>
        /// Optional. Collection of an item's combat stat modifiers when in a unit's inventory.
        /// </summary>
        public NamedStatConfig[] InventoryCombatStatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of an item's stat modifiers when in a unit's inventory.
        /// </summary>
        public NamedStatConfig[] InventoryStatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of locations of an item's engraving(s).
        /// </summary>
        public (int, int)[] Engravings { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Collection of locations of an item's tag(s).
        /// </summary>
        public (int, int)[] Tags { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Collection of locations of an item's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Location of an item's graphic image URL.
        /// </summary>
        public (int, int) GraphicURL { get; set; } = (-1, -1);

        #endregion Optional Fields
    }
}