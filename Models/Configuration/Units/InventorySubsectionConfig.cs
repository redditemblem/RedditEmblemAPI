using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class InventorySubsectionConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. List of container objects for the unit's inventory items.
        /// </summary>
        [JsonRequired]
        public List<UnitInventoryItemConfig> Slots { get; set; }

        #endregion Required Fields
    }
}
