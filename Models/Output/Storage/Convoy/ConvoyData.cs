using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Storage.Convoy
{
    /// <summary>
    /// Wrapper class for the serialized JSON convoy object data.
    /// </summary>
    public class ConvoyData
    {
        #region Attributes

        /// <summary>
        /// Container for currency constants.
        /// </summary>
        public CurrencyConstsConfig Currency { get; set; }

        /// <summary>
        /// Parameters for the Convoy page display.
        /// </summary>
        public FilterParameters Parameters { get; set; }
        
        /// <summary>
        /// Flag indicating if the shop link should be shown.
        /// </summary>
        public bool ShowShopLink { get; set; }

        /// <summary>
        /// List of <c>ConvoyItem</c>s that will be presented on the Convoy page.
        /// </summary>
        public IList<ConvoyItem> ConvoyItems { get; set; }

        /// <summary>
        /// List of <c>Item</c>s linked by the values in <c>ConvoyItems</c>.
        /// </summary>
        public IDictionary<string, Item> Items { get; set; }

        /// <summary>
        /// Container dictionary for data about tags.
        /// </summary>
        public IDictionary<string, Tag> Tags { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="ItemProcessingException"></exception>
        /// <exception cref="ConvoyItemProcessingException"></exception>
        public ConvoyData(JSONConfiguration config)
        {
            this.Currency = config.System.Currency;
            this.ShowShopLink = (config.Shop != null);

            if (config.System.Tags != null) this.Tags = Tag.BuildDictionary(config.System.Tags);
            else this.Tags = new Dictionary<string, Tag>();

            this.Items = Item.BuildDictionary(config.System.Items, this.Tags);

            //Build the convoy item list
            this.ConvoyItems = new List<ConvoyItem>();
            foreach (IList<object> row in config.Convoy.Query.Data)
            {
                try
                {
                    IList<string> item = row.Select(r => r.ToString()).ToList();
                    string name = ParseHelper.SafeStringParse(item, config.Convoy.Name, "Name", false);
                    if (string.IsNullOrEmpty(name)) continue;
                    this.ConvoyItems.Add(new ConvoyItem(config.Convoy, item, this.Items));
                }
                catch (Exception ex)
                {
                    throw new ConvoyItemProcessingException((row.ElementAtOrDefault(config.Convoy.Name) ?? string.Empty).ToString(), ex);
                }
            }

            //Build page parameters
            IList<ItemSort> sorts = new List<ItemSort>() {
                new ItemSort("Name", "name", false), 
                new ItemSort("Owner", "owner", false), 
                new ItemSort("Category", "category", true), 
                new ItemSort("Uses", "maxUses", true) 
            };
            if (config.System.WeaponRanks.Count > 0)
                sorts.Add(new ItemSort("Weapon Rank", "weaponRank", true));

            this.Parameters = new FilterParameters(sorts,
                new List<string>() { "All" }.Union(this.ConvoyItems.Select(i => i.Owner).Where(o => !string.IsNullOrEmpty(o)).Distinct().OrderBy(o => o)).ToList(),
                this.ConvoyItems.Select(i => i.Item.Category).Distinct().OrderBy(c => c).ToList(), 
                this.ConvoyItems.SelectMany(i => i.Item.UtilizedStats).Where(s => !string.IsNullOrEmpty(s)).Distinct().OrderBy(c => c).ToList(),
                new Dictionary<string, bool>());

            //Always do this last
            RemoveUnusedObjects();
        }

        /// <summary>
        /// Cull unneeded objects for JSON minification.
        /// </summary>
        private void RemoveUnusedObjects()
        {
            //Cull unused items
            foreach (string key in this.Items.Keys.ToList())
                if (!this.Items[key].Matched)
                    this.Items.Remove(key);

            //Cull unused tags
            foreach (string key in this.Tags.Keys.ToList())
                if (!this.Tags[key].Matched)
                    this.Tags.Remove(key);
        }
    }
}
