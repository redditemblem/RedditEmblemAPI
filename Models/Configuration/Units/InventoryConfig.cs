﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Inventory"</c> object data.
    /// </summary>
    public class InventoryConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index of a unit's primary equipped item name.
        /// </summary>
        [JsonRequired]
        public int PrimaryEquippedItem { get; set; }

        /// <summary>
        /// Required. List of container objects for the unit's inventory items.
        /// </summary>
        [JsonRequired]
        public List<UnitInventoryItemConfig> Slots { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. List of cell indexes for a unit's secondary equipped item names.
        /// </summary>
        public List<int> SecondaryEquippedItems { get; set; } = new List<int>();

        #endregion
    }
}