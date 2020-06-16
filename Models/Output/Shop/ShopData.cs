using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Exceptions.Processing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Shop
{
    /// <summary>
    /// Wrapper class for the serialized JSON shop object data.
    /// </summary>
    public class ShopData
    {
        public CurrencyConstsConfig Currency { get; set; }

        public FilterParameters Parameters { get; set; }

        public IList<ShopItem> ShopItems { get; set; }

        public IDictionary<string, Item> Items { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShopData(JSONConfiguration config)
        {
            this.Currency = config.System.Currency;

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

            //Build filters
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

            RemoveUnusedObjects();
        }
        private void RemoveUnusedObjects()
        {
            //Cull unused classes
            foreach (string key in this.Items.Keys.ToList())
                if (!this.Items[key].Matched)
                    this.Items.Remove(key);
        }
    }
}
