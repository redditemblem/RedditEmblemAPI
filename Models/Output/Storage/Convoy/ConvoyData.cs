using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Models.Output.System.Skills;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Storage.Convoy
{
    /// <summary>
    /// Wrapper class for the serialized JSON convoy object data.
    /// </summary>
    public class ConvoyData
    {
        #region Constants

        /// <summary>
        /// Container for currency constants.
        /// </summary>
        public CurrencyConstsConfig Currency { get; set; }

        /// <summary>
        /// Container for interface label constants.
        /// </summary>
        public InterfaceLabelsConfig InterfaceLabels { get; set; }

        #endregion Constants

        #region Attributes

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
        /// Container dictionary for data about skills.
        /// </summary>
        public IDictionary<string, Skill> Skills { get; set; }

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
        public ConvoyData(JSONConfiguration config)
        {
            this.Currency = config.System.Constants.Currency;
            this.InterfaceLabels = config.System.InterfaceLabels;

            this.WorkbookID = (config.Team.AlternativeWorkbookID.Length > 0 ? config.Team.AlternativeWorkbookID : config.Team.WorkbookID);
            this.ShowShopLink = (config.Shop != null);

            this.Skills = Skill.BuildDictionary(config.System.Skills);
            this.Tags = Tag.BuildDictionary(config.System.Tags);
            this.Engravings = Engraving.BuildDictionary(config.System.Engravings, this.Tags);
            this.Items = Item.BuildDictionary(config.System.Items, this.Skills, this.Tags, this.Engravings);
            this.ConvoyItems = ConvoyItem.BuildList(config.Convoy, this.Items, this.Engravings);

            //Build page parameters
            List<ItemSort> sorts = new List<ItemSort>() {
                new ItemSort("Name", "name", false),
                new ItemSort("Owner", "owner", false),
                new ItemSort("Category", "category", true),
                new ItemSort("Uses", "maxUses", true)
            };
            if (config.System.Constants.WeaponRanks.Count > 0)
                sorts.Add(new ItemSort(this.InterfaceLabels.WeaponRanks, "weaponRank", true));

            IDictionary<string, bool> filters = new Dictionary<string, bool>();
            filters.Add("AllowEngravings", config.Convoy.Engravings.Any() || config.System.Items.Engravings.Any());
            filters.Add("AllowEquippedSkills", config.System.Items.EquippedSkills.Any());

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
            CullDictionary(this.Skills);
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
