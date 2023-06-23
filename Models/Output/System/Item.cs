using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    /// <summary>
    /// Object representing an item definition in the team's system. 
    /// </summary>
    public class Item : IMatchable
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
        [JsonIgnore]
        public IDictionary<string, decimal> Stats { get; set; }

        /// <summary>
        /// Collection of combat stat modifiers that will be applied to the owning unit when this item is equipped.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> EquippedCombatStatModifiers { get; set; }

        /// <summary>
        /// Collection of stat modifiers that will be applied to the owning unit when this item is equipped.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> EquippedStatModifiers { get; set; }

        /// <summary>
        /// Collection of combat stat modifiers that will be applied to the owning unit when this item is not equipped.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> InventoryCombatStatModifiers { get; set; }

        /// <summary>
        /// Collection of stat modifiers that will be applied to the owning unit when this item is not equipped.
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
        [JsonIgnore]
        public List<Tag> TagsList { get; set; }

        /// <summary>
        /// The item's engravings.
        /// </summary>
        [JsonIgnore]
        public List<Engraving> Engravings { get; set; }

        /// <summary>
        /// List of the item's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #region JSON Serialization Only

        [JsonProperty]
        private IEnumerable<string> Tags { get { return this.TagsList.Select(t => t.Name); } }

        #endregion JSON Serialization Only

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Item(ItemsConfig config, IEnumerable<string> data, IDictionary<string, Tag> tags, IDictionary<string, Engraving> engravings)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.Category = DataParser.String(data, config.Category, "Category");
            this.WeaponRank = DataParser.OptionalString(data, config.WeaponRank, "Weapon Rank");
            this.UtilizedStats = DataParser.List_StringCSV(data, config.UtilizedStats);
            this.DealsDamage = DataParser.OptionalBoolean_YesNo(data, config.DealsDamage, "Deals Damage");
            this.MaxUses = DataParser.OptionalInt_Positive(data, config.Uses, "Uses");
            this.Range = new ItemRange(config.Range, data);
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            IEnumerable<string> itemTags = DataParser.List_StringCSV(data, config.Tags).Distinct();
            this.TagsList = Tag.MatchNames(tags, itemTags, true);

            IEnumerable<string> itemEngravings = DataParser.List_Strings(data, config.Engravings).Distinct();
            this.Engravings = Engraving.MatchNames(engravings, itemEngravings, true);

            this.Stats = DataParser.NamedStatDictionary_OptionalDecimal_Any(config.Stats, data, true);
            this.EquippedCombatStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.EquippedCombatStatModifiers, data, false, "{0} (Equipped)");
            this.EquippedStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.EquippedStatModifiers, data, false, "{0} (Equipped)");
            this.InventoryCombatStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.InventoryCombatStatModifiers, data, false, "{0} (Inventory)");
            this.InventoryStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.InventoryStatModifiers, data, false, "{0} (Inventory)");
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds an <c>Item</c> from each valid row.
        /// </summary>
        /// <exception cref="ItemProcessingException"></exception>
        public static IDictionary<string, Item> BuildDictionary(ItemsConfig config, IDictionary<string, Tag> tags, IDictionary<string, Engraving> engravings)
        {
            IDictionary<string, Item> items = new Dictionary<string, Item>();
            if (config == null || config.Query == null)
                return items;

            foreach (List<object> row in config.Query.Data)
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> item = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(item, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!items.TryAdd(name, new Item(config, item, tags, engravings)))
                        throw new NonUniqueObjectNameException("item");
                }
                catch (Exception ex)
                {
                    throw new ItemProcessingException(name, ex);
                }
            }

            return items;
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to a <c>Item</c> in <paramref name="items"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Item> MatchNames(IDictionary<string, Item> items, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(items, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>Item</c> in <paramref name="items"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedItemException"></exception>
        public static Item MatchName(IDictionary<string, Item> items, string name, bool skipMatchedStatusSet = false)
        {
            Item match;
            if (!items.TryGetValue(name, out match))
                throw new UnmatchedItemException(name);

            if (!skipMatchedStatusSet)
            {
                match.Matched = true;
                match.TagsList.ForEach(t => t.Matched = true);
                match.Engravings.ForEach(e => e.Matched = true);
            }

            return match;
        }

        #endregion
    }
}