using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Interfaces;
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
        /// Workbook ID number from the Google Sheets URL.
        /// </summary>
        public string WorkbookID { get; set; }

        /// <summary>
        /// Flag indicating if the shop link should be shown.
        /// </summary>
        public bool ShowShopLink { get; set; }

        /// <summary>
        /// List of <c>ConvoyItem</c>s that will be presented on the Convoy page.
        /// </summary>
        public List<ConvoyItem> ConvoyItems { get; set; }

        /// <summary>
        /// List of <c>Item</c>s linked by the values in <c>ConvoyItems</c>.
        /// </summary>
        public IDictionary<string, Item> Items { get; set; }

        /// <summary>
        /// Container dictionary for data about tags.
        /// </summary>
        public IDictionary<string, Tag> Tags { get; set; }

        /// <summary>
        /// Container dictionary for data about engravings.
        /// </summary>
        public IDictionary<string, Engraving> Engravings { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="ConvoyItemProcessingException"></exception>
        public ConvoyData(JSONConfiguration config)
        {
            this.Currency = config.System.Currency;
            this.WorkbookID = config.Team.WorkbookID;
            this.ShowShopLink = (config.Shop != null);

            this.Tags = Tag.BuildDictionary(config.System.Tags);
            this.Engravings = Engraving.BuildDictionary(config.System.Engravings);
            this.Items = Item.BuildDictionary(config.System.Items, this.Tags, this.Engravings);

            //Build the convoy item list
            this.ConvoyItems = new List<ConvoyItem>();
            foreach (List<object> row in config.Convoy.Query.Data)
            {
                try
                {
                    IEnumerable<string> item = row.Select(r => r.ToString());
                    string name = DataParser.OptionalString(item, config.Convoy.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    this.ConvoyItems.Add(new ConvoyItem(config.Convoy, item, this.Items, this.Engravings));
                }
                catch (Exception ex)
                {
                    throw new ConvoyItemProcessingException((row.ElementAtOrDefault(config.Convoy.Name) ?? string.Empty).ToString(), ex);
                }
            }

            //Build page parameters
            List<ItemSort> sorts = new List<ItemSort>() {
                new ItemSort("Name", "name", false),
                new ItemSort("Owner", "owner", false),
                new ItemSort("Category", "category", true),
                new ItemSort("Uses", "maxUses", true)
            };
            if (config.System.WeaponRanks.Count > 0)
                sorts.Add(new ItemSort("Weapon Rank", "weaponRank", true));

            IDictionary<string, bool> filters = new Dictionary<string, bool>();
            filters.Add("AllowEngravings", config.Convoy.Engravings.Any() || config.System.Items.Engravings.Any());

            this.Parameters = new FilterParameters(sorts,
                new List<string>() { "All" }.Union(this.ConvoyItems.Select(i => i.Owner).Where(o => !string.IsNullOrEmpty(o)).Distinct().OrderBy(o => o)),
                this.ConvoyItems.Select(i => i.Item.Category).Distinct().OrderBy(c => c),
                this.ConvoyItems.SelectMany(i => i.Item.UtilizedStats).Where(s => !string.IsNullOrEmpty(s)).Distinct().OrderBy(c => c),
                filters);

            //Always do this last
            RemoveUnusedObjects();
        }

        /// <summary>
        /// Cull unneeded objects for JSON minification.
        /// </summary>
        private void RemoveUnusedObjects()
        {
            CullDictionary(this.Items);
            CullDictionary(this.Tags);
            CullDictionary(this.Engravings);
        }

        private void CullDictionary<T>(IDictionary<string, T> dictionary) where T : IMatchable
        {
            foreach (string key in dictionary.Keys)
                if (!dictionary[key].Matched)
                    dictionary.Remove(key);
        }
    }
}
