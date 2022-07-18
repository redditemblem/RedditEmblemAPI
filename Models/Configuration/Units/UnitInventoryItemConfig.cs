using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitInventoryItemConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index of the inventory item's name.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of the inventory item's remaining uses.
        /// </summary>
        public int Uses { get; set; } = -1;

        #endregion Optional Fields
    }
}
