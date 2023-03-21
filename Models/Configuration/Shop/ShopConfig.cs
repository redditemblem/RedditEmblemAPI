using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Shop
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Shop"</c> object data.
    /// </summary>
    public class ShopConfig : IQueryable
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index for a shop item's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required.  Cell index for a shop item's price value.
        /// </summary>
        [JsonRequired]
        public int Price { get; set; }

        /// <summary>
        /// Required. Cell index for a shop item's stock value.
        /// </summary>
        [JsonRequired]
        public int Stock { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index for a shop item's sale price value.
        /// </summary>
        public int SalePrice { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index for a shop item's is new flag.
        /// </summary>
        public int IsNew { get; set; } = -1;

        /// <summary>
        /// Optional. Cell indexes for a shop item's engravings.
        /// </summary>
        public List<int> Engravings { get; set; } = new List<int>();

        #endregion
    }
}
