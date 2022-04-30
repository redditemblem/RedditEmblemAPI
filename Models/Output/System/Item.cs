using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    /// <summary>
    /// Object representing an item definition in the team's system. 
    /// </summary>
    public class Item
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this item was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The item's name. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite image URL for the item.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// The item's category. (ex. Sword)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The minimum weapon rank required to wield the item, if applicable.
        /// </summary>
        public string WeaponRank { get; set; }

        /// <summary>
        /// The unit stats that the item uses in calculations, if applicable.
        /// </summary>
        public List<string> UtilizedStats { get; set; }

        /// <summary>
        /// Flag indicating whether or not this item is capable of attacking.
        /// </summary>
        public bool DealsDamage { get; set; }

        /// <summary>
        /// The maximum number of uses the item has. For items with single or infinite uses, this value is 0.
        /// </summary>
        public int MaxUses { get; set; }

        /// <summary>
        /// Collection of stat values for the item. (ex. Hit)
        /// </summary>
        public IDictionary<string, int> Stats { get; set; }

        /// <summary>
        /// Collection of stat modifiers that will be applied to the owning unit when this item <c>IsEquipped</c>.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> EquippedStatModifiers { get; set; }

        /// <summary>
        /// Collection of stat modifiers that will be applied to the owning unit when this item !<c>IsEquipped</c>.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> InventoryStatModifiers { get; set; }

        /// <summary>
        /// The item's range.
        /// </summary>
        public ItemRange Range { get; set; }

        /// <summary>
        /// The item's tags.
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// List of the item's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="AnyIntegerException"></exception>
        public Item(ItemsConfig config, List<string> data, IDictionary<string, Tag> tags)
        {
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.Category = DataParser.String(data, config.Category, "Category");
            this.WeaponRank = DataParser.OptionalString(data, config.WeaponRank, "Weapon Rank");
            this.UtilizedStats = DataParser.List_StringCSV(data, config.UtilizedStats);
            this.DealsDamage = DataParser.OptionalBoolean_YesNo(data, config.DealsDamage, "Deals Damage");
            this.MaxUses = DataParser.Int_Positive(data, config.Uses, "Uses");
            this.Range = new ItemRange(config.Range, data);
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            this.Tags = DataParser.List_StringCSV(data, config.Tags).Distinct().ToList();
            MatchTags(tags);

            this.Stats = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.Stats)
            {
                int val = DataParser.Int_Any(data, stat.Value, stat.SourceName);
                this.Stats.Add(stat.SourceName, val);
            }

            this.EquippedStatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.EquippedStatModifiers)
            {
                int val = DataParser.OptionalInt_Any(data, stat.Value, $"{stat.SourceName} (Equipped)");
                if (val == 0) continue;
                this.EquippedStatModifiers.Add(stat.SourceName, val);
            }

            this.InventoryStatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.InventoryStatModifiers)
            {
                int val = DataParser.OptionalInt_Any(data, stat.Value, $"{stat.SourceName} (Inventory)");
                if (val == 0) continue;
                this.InventoryStatModifiers.Add(stat.SourceName, val);
            }
        }

        /// <summary>
        /// MUST BE RUN AFTER TAGS ARE BUILT. Iterates through the values in <c>this.Tags</c> and attempts to match them a <c>Tag</c> from <paramref name="tags"/>.
        /// </summary>
        /// <param name="tags"></param>
        private void MatchTags(IDictionary<string, Tag> tags)
        {
            foreach (string tag in this.Tags)
            {
                Tag match;
                if (!tags.TryGetValue(tag, out match))
                    throw new UnmatchedTagException(tag);

                match.Matched = true;
            }
        }

        #region Static Functions

        public static IDictionary<string, Item> BuildDictionary(ItemsConfig config, IDictionary<string, Tag> tags)
        {
            IDictionary<string, Item> items = new Dictionary<string, Item>();

            foreach (List<object> row in config.Query.Data)
            {
                try
                {
                    List<string> item = row.Select(r => r.ToString()).ToList();
                    string name = DataParser.OptionalString(item, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!items.TryAdd(name, new Item(config, item, tags)))
                        throw new NonUniqueObjectNameException("item");
                }
                catch (Exception ex)
                {
                    throw new ItemProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            return items;
        }

        #endregion
    }
}