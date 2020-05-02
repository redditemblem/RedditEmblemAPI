using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    /// <summary>
    /// Object representing a Item definition in the team's system. 
    /// </summary>
    public class Item
    {
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
        /// The unit stat that the item uses in calculations, if applicable.
        /// </summary>
        public string UtilizedStat { get; set; }

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
        public Dictionary<string, int> Stats { get; set; }

        /// <summary>
        /// Collection of stat modifiers that will be applied to the owning unit when this item <c>IsEquipped</c>.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, int> EquippedStatModifiers { get; set; }

        /// <summary>
        /// Collection of stat modifiers that will be applied to the owning unit when this item !<c>IsEquipped</c>.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, int> InventoryStatModifiers { get; set; }

        /// <summary>
        /// The item's range.
        /// </summary>
        public ItemRange Range { get; set; }

        /// <summary>
        /// List of the item's text fields.
        /// </summary>
        public IList<string> TextFields { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="data"></param>
        /// <exception cref="AnyIntegerException"></exception>
        public Item(ItemsConfig config, IList<string> data)
        {
            this.Name = data.ElementAtOrDefault(config.Name).Trim();
            this.SpriteURL = data.ElementAtOrDefault<string>(config.SpriteURL).Trim();
            this.Category = data.ElementAtOrDefault<string>(config.Category);
            this.WeaponRank = (data.ElementAtOrDefault<string>(config.WeaponRank) ?? string.Empty);
            this.UtilizedStat = data.ElementAtOrDefault<string>(config.UtilizedStat);
            this.DealsDamage = ((data.ElementAtOrDefault(config.DealsDamage) ?? "No").Trim() == "Yes");
            this.MaxUses = ParseHelper.SafeIntParse(data.ElementAtOrDefault(config.Uses), "Uses", true);
            this.Range = new ItemRange(data.ElementAtOrDefault<string>(config.Range.Minimum),
                                       data.ElementAtOrDefault<string>(config.Range.Maximum));
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);

            this.Stats = new Dictionary<string, int>();
            foreach (NamedStatConfig s in config.Stats)
            {
                int val;
                if (!int.TryParse(data.ElementAtOrDefault(s.Value), out val))
                    throw new AnyIntegerException(s.SourceName, data.ElementAtOrDefault<string>(s.Value));

                this.Stats.Add(s.SourceName, val);
            }

            this.EquippedStatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.EquippedStatModifiers)
            {
                int val;
                if (!int.TryParse(data.ElementAtOrDefault(stat.Value), out val))
                    throw new AnyIntegerException(string.Format("{0} ({1})", stat.SourceName, "Equipped"), data.ElementAtOrDefault<string>(stat.Value));

                if (val != 0)
                    this.EquippedStatModifiers.Add(stat.SourceName, val);
            }

            this.InventoryStatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.InventoryStatModifiers)
            {
                int val;
                if (!int.TryParse(data.ElementAtOrDefault(stat.Value), out val))
                    throw new AnyIntegerException(string.Format("{0} ({1})", stat.SourceName, "Inventory"), data.ElementAtOrDefault<string>(stat.Value));

                if (val != 0)
                    this.InventoryStatModifiers.Add(stat.SourceName, val);
            }
        }
    }
}