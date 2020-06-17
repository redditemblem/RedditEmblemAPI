using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Shop;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Storage.Shop
{
    public class ShopItem
    {
        #region Attributes

        /// <summary>
        /// The full name of the item pulled from raw <c>ShopData</c> data.
        /// </summary>
        [JsonIgnore]
        public string FullName { get; set; }

        /// <summary>
        /// Only for JSON serialization. The name of the item.
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.Item.Name; } }

        /// <summary>
        /// The <c>Item</c> object.
        /// </summary>
        [JsonIgnore]
        public Item Item { get; set; }

        /// <summary>
        /// The purchase price of the item.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// If the item is on sale, the sale price. Else, the value of <c>Price</c>.
        /// </summary>
        public int SalePrice { get; set; }

        /// <summary>
        /// The number of items available for sale. A value of 99 indicates infinite stock.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Flag indicating if the item is a new addition to the shop.
        /// </summary>
        public bool IsNew { get; set; }

        #endregion

        /// <summary>
        /// Attempts to match the values in <paramref name="data"/> to an <c>Item</c> from <paramref name="items"/>.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="data"></param>
        /// <param name="items"></param>
        /// <exception cref="UnmatchedItemException"></exception>
        public ShopItem(ShopConfig config, IList<string> data, IDictionary<string, Item> items)
        {
            this.FullName = data.ElementAtOrDefault<string>(config.Name);

            Item match;
            if (!items.TryGetValue(data.ElementAtOrDefault<string>(config.Name), out match))
                throw new UnmatchedItemException(data.ElementAtOrDefault<string>(config.Name));
            this.Item = match;
            match.Matched = true;

            this.Price = ParseHelper.SafeIntParse(data.ElementAtOrDefault<string>(config.Price), "Price", true);
            this.SalePrice = ParseHelper.OptionalSafeIntParse(data.ElementAtOrDefault<string>(config.SalePrice), "Sale Price", true, this.Price);
            this.Stock = ParseHelper.SafeIntParse(data.ElementAtOrDefault<string>(config.Stock), "Stock", true);
            this.IsNew = ((data.ElementAtOrDefault<string>(config.IsNew) ?? string.Empty) == "Yes");
        }
    }
}
