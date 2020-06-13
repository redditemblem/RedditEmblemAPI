using Newtonsoft.Json;
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
        /// Cell index of a unit's equipped item name.
        /// </summary>
        [JsonRequired]
        public int EquippedItem { get; set; }

        /// <summary>
        /// List of cell indexes for a unit's inventory items.
        /// </summary>
        [JsonRequired]
        public IList<int> Slots { get; set; }

        #endregion
    }
}