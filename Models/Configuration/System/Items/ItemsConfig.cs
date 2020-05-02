using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Items
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Items"</c> object data.
    /// </summary>
    public class ItemsConfig
    {
        #region RequiredFields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Cell index of an item's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Cell index of an item's category value.
        /// </summary>
        [JsonRequired]
        public int Category { get; set; }

        /// <summary>
        /// Cell index of an item's utilized stat value.
        /// </summary>
        [JsonRequired]
        public int UtilizedStat { get; set; }

        /// <summary>
        /// Cell index of the item's deal damage flag.
        /// </summary>
        [JsonRequired]
        public int DealsDamage { get; set; }

        /// <summary>
        /// Cell index of an item's uses value.
        /// </summary>
        [JsonRequired]
        public int Uses { get; set; }

        /// <summary>
        /// List of an item's stat configurations.
        /// </summary>
        [JsonRequired]
        public IList<NamedStatConfig> Stats { get; set; }

        /// <summary>
        /// Container object for an item's range configuration.
        /// </summary>
        [JsonRequired]
        public RangeConfig Range { get; set; }

        #endregion

        #region OptionalFields

        /// <summary>
        /// Cell index of an item's sprite image URL value.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Cell index of an item's weapon rank value.
        /// </summary>
        public int WeaponRank { get; set; } = -1;

        /// <summary>
        /// List of an item's stat modifiers when equipped.
        /// </summary>
        public IList<NamedStatConfig> EquippedStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// List of an item's stat modifiers when in a unit's inventory.
        /// </summary>
        public IList<NamedStatConfig> InventoryStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// List of cell indexes for an item's text fields.
        /// </summary>
        public IList<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}