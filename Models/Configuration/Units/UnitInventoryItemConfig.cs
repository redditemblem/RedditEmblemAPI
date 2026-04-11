using Newtonsoft.Json;
using System;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitInventoryItemConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of the inventory item's name.
        /// </summary>
        [JsonRequired]
        public (int, int) Name { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of the inventory item's remaining uses.
        /// </summary>
        public (int, int) Uses { get; set; } = (-1, -1);

        ///<summary>
        ///Optional. Collection of locations of engravings on the inventory item.
        ///</summary>
        public (int, int)[] Engravings { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
