using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.Shop
{
    public class ShopConfig
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Cell index for the shop item's name.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Cell index for the shop item's price.
        /// </summary>
        [JsonRequired]
        public int Price { get; set; }

        /// <summary>
        /// Cell index for the shop item's stock.
        /// </summary>
        [JsonRequired]
        public int Stock { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Cell index for the sale price of the shop item.
        /// </summary>
        public int SalePrice { get; set; } = -1;

        /// <summary>
        /// Cell index for flag indicating if the shop item is new.
        /// </summary>
        public int IsNew { get; set; } = -1;

        #endregion
    }
}
