using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Output.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Storage.Shop
{
    /// <summary>
    /// Wrapper class for the serialized JSON shop object data.
    /// </summary>
    public class ShopData
    {
        #region Attributes

        /// <summary>
        /// Container for currency constants.
        /// </summary>
        public CurrencyConstsConfig Currency { get; set; }

        /// <summary>
        /// Parameters for the Shop page display.
        /// </summary>
        public FilterParameters Parameters { get; set; }

        /// <summary>
        /// List of <c>ShopItem</c>s that will be presented on the Shop page.
        /// </summary>
        public IList<ShopItem> ShopItems { get; set; }

        /// <summary>
        /// List of <c>Item</c>s linked by the values in <c>ShopItems</c>.
        /// </summary>
        public IDictionary<string, Item> Items { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="ItemProcessingException"></exception>
        /// <exception cref="ShopItemProcessingException"></exception>
        public ShopData(JSONConfiguration config)
        {
            this.Currency = config.System.Currency;

            //Build the item list
            this.Items = new Dictionary<string, Item>();
            foreach (IList<object> row in config.System.Items.Query.Data)
            {
                try
                {
                    IList<string> item = row.Select(r => r.ToString()).ToList();
                    if (string.IsNullOrEmpty(item.ElementAtOrDefault<string>(config.System.Items.Name)))
                        continue;
                    this.Items.Add(item.ElementAtOrDefault(config.System.Items.Name), new Item(config.System.Items, item));
                }
                catch (Exception ex)
                {
                    throw new ItemProcessingException((row.ElementAtOrDefault(config.System.Items.Name) ?? string.Empty).ToString(), ex);
                }
            }

            //Build the shop item list
            this.ShopItems = new List<ShopItem>();
            foreach (IList<object> row in config.Shop.Query.Data)
            {
                try
                {
                    IList<string> item = row.Select(r => r.ToString()).ToList();
                    if (string.IsNullOrEmpty(item.ElementAtOrDefault<string>(config.Shop.Name)))
                        continue;
                    this.ShopItems.Add(new ShopItem(config.Shop, item, this.Items));
                }
                catch (Exception ex)
                {
                    throw new ShopItemProcessingException((row.ElementAtOrDefault(config.Shop.Name) ?? string.Empty).ToString(), ex);
                }
            }

            //Build page parameters
            IList<ItemSort> sorts = new List<ItemSort>() {
                new ItemSort("Name", "name", false),
                new ItemSort("Price", "price", false),
                new ItemSort("Category", "category", true)
            };
            if (config.System.WeaponRanks.Count > 0)
                sorts.Add(new ItemSort("Weapon Rank", "weaponRank", true));

            IDictionary<string, bool> filters = new Dictionary<string, bool>();
            filters.Add("AllowNew", (config.Shop.IsNew != -1));
            filters.Add("AllowSales", (config.Shop.SalePrice != -1));

            this.Parameters = new FilterParameters(sorts,
                this.ShopItems.Select(i => i.Item.Category).Distinct().OrderBy(c => c).ToList(),
                this.ShopItems.Select(i => i.Item.UtilizedStat).Where(s => !string.IsNullOrEmpty(s)).Distinct().OrderBy(c => c).ToList(),
                filters);

            //Always do this last.
            RemoveUnusedObjects();
        }

        /// <summary>
        /// Cull unneeded objects for JSON minification.
        /// </summary>
        private void RemoveUnusedObjects()
        {
            //Cull unused classes
            foreach (string key in this.Items.Keys.ToList())
                if (!this.Items[key].Matched)
                    this.Items.Remove(key);
        }
    }
}
