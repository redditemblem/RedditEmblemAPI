using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Shop;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditEmblemAPI.Models.Output.Shop
{
    public class ShopItem
    {
        /// <summary>
        /// Only for JSON serialization. The name of the item.
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.Item.Name; } }

        /// <summary>
        /// The item.
        /// </summary>
        [JsonIgnore]
        public Item Item { get; set; }

        public int Price { get; set; }

        public int Stock { get; set; }

        public ShopItem(ShopConfig config, IList<string> data, IDictionary<string, Item> items)
        {
            Item match;
            if (!items.TryGetValue(data.ElementAtOrDefault<string>(config.Name), out match))
                throw new UnmatchedShopItemException(data.ElementAtOrDefault<string>(config.Name));
            this.Item = match;
            match.Matched = true;

            this.Price = ParseHelper.SafeIntParse(data.ElementAtOrDefault<string>(config.Price), "Price", true);
            this.Stock = ParseHelper.SafeIntParse(data.ElementAtOrDefault<string>(config.Stock), "Stock", true);
        }
    }
}
