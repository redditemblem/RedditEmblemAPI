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
        /// Required. List of the sections in the inventory.
        /// </summary>
        [JsonRequired]
        public IList<InventorySectionConfig> Sections { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. The overarching header title for the Inventory section in the UI. Defaults to "Inventory".
        /// </summary>
        public string InventoryTitle { get; set; } = "Inventory";

        #endregion
    }
}