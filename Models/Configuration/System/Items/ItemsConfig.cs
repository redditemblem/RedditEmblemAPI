﻿using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Items
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Items"</c> object data.
    /// </summary>
    public class ItemsConfig
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
        public int UtilizedStats { get; set; }

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
        public IList<NamedStatConfig> Stats { get; set; }

        /// <summary>
        /// Required. Container object for an item's range configuration.
        /// </summary>
        [JsonRequired]
        public RangeConfig Range { get; set; }

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
        /// Optional. List of an item's stat modifiers when equipped.
        /// </summary>
        public IList<NamedStatConfig> EquippedStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of an item's stat modifiers when in a unit's inventory.
        /// </summary>
        public IList<NamedStatConfig> InventoryStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. Cell index of an item's tags.
        /// </summary>
        public int Tags { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for an item's text fields.
        /// </summary>
        public IList<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}