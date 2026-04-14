using Newtonsoft.Json;
using System;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Inventory"</c> object data.
    /// </summary>
    public class InventoryConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a unit's primary equipped item name.
        /// </summary>
        [JsonRequired]
        public (int, int) PrimaryEquippedItem { get; set; }

        /// <summary>
        /// Required. Collection of a unit's inventory subsections.
        /// </summary>
        [JsonRequired]
        public InventorySubsectionConfig[] Subsections { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Collection of locations of a unit's secondary equipped item names.
        /// </summary>
        public (int, int)[] SecondaryEquippedItems { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}