using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Exceptions.Processing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Convoy
{
    public class ConvoyData
    {
        public CurrencyConstsConfig Currency { get; set; }

        public FilterParameters Parameters { get; set; }

        public IList<ConvoyItem> ConvoyItems { get; set; }

        public IDictionary<string, Item> Items { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConvoyData(JSONConfiguration config)
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

            this.ConvoyItems = new List<ConvoyItem>();
            foreach (IList<object> row in config.Convoy.Query.Data)
            {
                try
                {
                    IList<string> item = row.Select(r => r.ToString()).ToList();
                    if (string.IsNullOrEmpty(item.ElementAtOrDefault<string>(config.Convoy.Name)))
                        continue;
                    this.ConvoyItems.Add(new ConvoyItem(config.Convoy, item, this.Items));
                }
                catch (Exception ex)
                {
                    throw new ConvoyItemProcessingException((row.ElementAtOrDefault(config.Convoy.Name) ?? string.Empty).ToString(), ex);
                }
            }

            //Build filters
            IList<ItemSort> sorts = new List<ItemSort>() {
                new ItemSort("Name", "name", false), 
                new ItemSort("Owner", "owner", false), 
                new ItemSort("Category", "category", true), 
                new ItemSort("Uses", "maxUses", true) 
            };

            if (config.System.WeaponRanks.Count > 0)
                sorts.Add(new ItemSort("Weapon Rank", "weaponRank", true));

            this.Parameters = new FilterParameters(sorts,
                this.ConvoyItems.Select(i => i.Item.Category).Distinct().OrderBy(c => c).ToList(), 
                this.ConvoyItems.Select(i => i.Item.UtilizedStat).Where(s => !string.IsNullOrEmpty(s)).Distinct().OrderBy(c => c).ToList(),
                new Dictionary<string, bool>());

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
