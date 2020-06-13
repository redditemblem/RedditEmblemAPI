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
    }
}
