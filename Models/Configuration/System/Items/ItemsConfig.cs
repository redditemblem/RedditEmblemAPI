using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Items
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Items"</c> object data.
    /// </summary>
    public class ItemsConfig : IQueryable
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of an item's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required. Cell index of an item's category value.
        /// </summary>
        [JsonRequired]
        public int Category { get; set; }

        /// <summary>
        /// Required. Cell index of an item's utilized stat value.
        /// </summary>
        [JsonRequired]
        public List<int> UtilizedStats { get; set; }

        /// <summary>
        /// Required. Cell index of an item's deal damage flag.
        /// </summary>
        [JsonRequired]
        public int DealsDamage { get; set; }

        /// <summary>
        /// Required. Cell index of an item's uses value.
        /// </summary>
        [JsonRequired]
        public int Uses { get; set; }

        /// <summary>
        /// Required. List of an item's stat configurations.
        /// </summary>
        [JsonRequired]
        public List<NamedStatConfig> Stats { get; set; }

        /// <summary>
        /// Required. Container object for an item's range configuration.
        /// </summary>
        [JsonRequired]
        public ItemRangeConfig Range { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of an item's sprite image URL value.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of an item's weapon rank value.
        /// </summary>
        public int WeaponRank { get; set; } = -1;

        /// <summary>
        /// Optional. List of an item's combat stat modifiers when equipped.
        /// </summary>
        public List<NamedStatConfig> EquippedCombatStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of an item's stat modifiers when equipped.
        /// </summary>
        public List<NamedStatConfig> EquippedStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of an item's skills that activate when equipped.
        /// </summary>
        public List<UnitSkillConfig> EquippedSkills { get; set; } = new List<UnitSkillConfig>();

        /// <summary>
        /// Optional. List of an item's combat stat modifiers when in a unit's inventory.
        /// </summary>
        public List<NamedStatConfig> InventoryCombatStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of an item's stat modifiers when in a unit's inventory.
        /// </summary>
        public List<NamedStatConfig> InventoryStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of cell indexes for an item's engraving(s).
        /// </summary>
        public List<int> Engravings { get; set; } = new List<int>();

        /// <summary>
        /// Optional. List of cell indexes for an item's tag(s).
        /// </summary>
        public List<int> Tags { get; set; } = new List<int>();

        /// <summary>
        /// Optional. List of cell indexes for an item's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}