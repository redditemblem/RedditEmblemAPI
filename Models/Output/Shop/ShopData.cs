using RedditEmblemAPI.Models.Configuration.Shop;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Exceptions.Processing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Shop
{
    public class ShopData
    {
        public IList<ShopItem> ShopItems { get; set; }

        public IDictionary<string, Item> Items { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShopData(ShopConfig shopConfig, ItemsConfig itemsConfig)
        {
            this.Items = new Dictionary<string, Item>();
            foreach (IList<object> row in itemsConfig.Query.Data)
            {
                try
                {
                    IList<string> item = row.Select(r => r.ToString()).ToList();
                    if (string.IsNullOrEmpty(item.ElementAtOrDefault<string>(itemsConfig.Name)))
                        continue;
                    this.Items.Add(item.ElementAtOrDefault(itemsConfig.Name), new Item(itemsConfig, item));
                }
                catch (Exception ex)
                {
                    throw new ItemProcessingException((row.ElementAtOrDefault(itemsConfig.Name) ?? string.Empty).ToString(), ex);
                }
            }

            this.ShopItems = new List<ShopItem>();
            foreach (IList<object> row in shopConfig.Query.Data)
            {
                try
                {
                    IList<string> item = row.Select(r => r.ToString()).ToList();
                    if (string.IsNullOrEmpty(item.ElementAtOrDefault<string>(shopConfig.Name)))
                        continue;
                    this.ShopItems.Add(new ShopItem(shopConfig, item, this.Items));
                }
                catch (Exception ex)
                {
                    throw new ShopItemProcessingException((row.ElementAtOrDefault(shopConfig.Name) ?? string.Empty).ToString(), ex);
                }
            }

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
