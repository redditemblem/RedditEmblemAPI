using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Convoy;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output.Storage.Convoy
{
    public class ConvoyItem
    {
        #region Attributes

        /// <summary>
        /// The full name of the item pulled from raw convoy data.
        /// </summary>
        [JsonIgnore]
        public string FullName { get; set; }

        [JsonIgnore]
        public Item Item { get; set; }

        /// <summary>
        /// The player that owns this item.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// The number of uses the item currently has remaining.
        /// </summary>
        public int Uses { get; set; }

        /// <summary>
        /// Dictionary of the item's stats. Copied over from <c>Item</c> on match.
        /// </summary>
        public IDictionary<string, UnitInventoryItemStat> Stats { get; set; }

        /// <summary>
        /// The number of this item present in the convoy.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The sell value of this item.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// List of the item's tags.
        /// </summary>
        [JsonIgnore]
        public List<Tag> TagsList { get; set; }

        /// <summary>
        /// List of the engravings applied to the item.
        /// </summary>
        [JsonIgnore]
        public List<Engraving> EngravingsList { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// Only for JSON serialization. The name of the item.
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.Item.Name; } }

        /// <summary>
        /// For JSON serialization only. Names of the item's tags.
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> Tags { get { return this.TagsList.Select(t => t.Name); } }

        /// <summary>
        /// For JSON serialization only. Complete list of the item's engravings.
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> Engravings { get { return this.EngravingsList.Select(e => e.Name).Union(this.Item.Engravings.Select(e => e.Name)).Distinct(); } }

        #endregion JSON Serialization Only

        #endregion Attributes

        private static Regex usesRegex = new Regex(@"\([0-9]+\)"); //match item uses (ex. "(5)")

        /// <summary>
        /// Constructor. Builds the <c>ConvoyItem</c> and matches it to an <c>Item</c> definition from <paramref name="items"/>.
        /// </summary>
        public ConvoyItem(ConvoyConfig config, IEnumerable<string> data, IReadOnlyDictionary<string, Item> items, IReadOnlyDictionary<string, Engraving> engravings)
        {
            this.FullName = DataParser.String(data, config.Name, "Name");
            this.Uses = 0;

            string name = this.FullName;
            if(config.Uses > -1)
            {
                this.Uses = DataParser.OptionalInt_Positive(data, config.Uses, "Uses");
            }
            else
            {
                //Search for uses syntax
                Match usesMatch = usesRegex.Match(name);
                if (usesMatch.Success)
                {
                    //Convert item use synatax to int
                    string u = usesMatch.Value.ToString();
                    u = u.Substring(1, u.Length - 2);
                    this.Uses = int.Parse(u);
                    name = usesRegex.Replace(name, string.Empty);
                }
            }
            
            name = name.Trim();
            this.Item = Item.MatchName(items, name);

            //Copy data from parent item
            this.Stats = new Dictionary<string, UnitInventoryItemStat>();
            foreach (KeyValuePair<string, NamedStatValue> stat in this.Item.Stats)
                this.Stats.Add(stat.Key, new UnitInventoryItemStat(stat.Value));
            this.TagsList = this.Item.Tags.ToList();

            this.Owner = DataParser.OptionalString(data, config.Owner, "Owner");
            this.Value = DataParser.OptionalInt_Positive(data, config.Value, "Value", -1);
            this.Quantity = DataParser.OptionalInt_NonZeroPositive(data, config.Quantity, "Quantity");

            List<string> itemEngravings = DataParser.List_Strings(data, config.Engravings);
            this.EngravingsList = Engraving.MatchNames(engravings, itemEngravings);

            ApplyEngravings();
        }

        private void ApplyEngravings()
        {
            foreach (Engraving engraving in this.EngravingsList.Union(this.Item.Engravings))
            {
                //Apply any modifiers to the item's stats
                foreach (KeyValuePair<string, int> mod in engraving.ItemStatModifiers)
                {
                    UnitInventoryItemStat stat = MatchStatName(mod.Key);
                    stat.Modifiers.TryAdd(engraving.Name, mod.Value);
                }

                //Apply any tags
                this.TagsList = this.TagsList.Union(engraving.Tags).ToList();
            }
        }

        public UnitInventoryItemStat MatchStatName(string name)
        {
            UnitInventoryItemStat stat;
            if (!this.Stats.TryGetValue(name, out stat))
                throw new UnmatchedStatException(name);

            return stat;
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>ConvoyItem</c> from each valid row.
        /// </summary>
        /// <exception cref="ConvoyItemProcessingException"></exception>
        public static List<ConvoyItem> BuildList(ConvoyConfig config, IReadOnlyDictionary<string, Item> items, IReadOnlyDictionary<string, Engraving> engravings)
        {
            List<ConvoyItem> convoyItems = new List<ConvoyItem>();
            if (config == null || config.Query == null)
                return convoyItems;

            foreach (List<object> row in config.Query.Data)
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> convoyItem = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(convoyItem, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    convoyItems.Add(new ConvoyItem(config, convoyItem, items, engravings));
                }
                catch (Exception ex)
                {
                    throw new ConvoyItemProcessingException(name, ex);
                }
            }

            return convoyItems;
        }

        #endregion Static Functions
    }
}
