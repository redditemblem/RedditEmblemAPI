using Newtonsoft.Json;
using System.Collections.Generic;

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

        ///<summary>
        ///Optional. Cell indexes of engravings on the inventory item.
        ///</summary>
        public List<int> Engravings { get; set; } = new List<int>();

        #endregion Optional Fields
    }
}
