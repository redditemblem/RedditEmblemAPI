using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class InventorySubsectionConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Collection of container objects for the unit's inventory items.
        /// </summary>
        [JsonRequired]
        public UnitInventoryItemConfig[] Slots { get; set; }

        #endregion Required Fields
    }
}
