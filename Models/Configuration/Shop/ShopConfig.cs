using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.Shop
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Shop"</c> object data.
    /// </summary>
    public class ShopConfig : Queryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a shop item's name value.
        /// </summary>
        [JsonRequired]
        public (int, int) Name { get; set; }

        /// <summary>
        /// Required. Location of a shop item's price value.
        /// </summary>
        [JsonRequired]
        public (int, int) Price { get; set; }

        /// <summary>
        /// Required. Location of a shop item's stock value.
        /// </summary>
        [JsonRequired]
        public (int, int) Stock { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of a shop item's sale price value.
        /// </summary>
        public (int, int) SalePrice { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a shop item's is new flag.
        /// </summary>
        public (int, int) IsNew { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Cell indexes for a shop item's engravings.
        /// </summary>
        public (int, int)[] Engravings { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
