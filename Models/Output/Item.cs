using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    /// <summary>
    /// Object representing a Item definition in the team's system. 
    /// </summary>
    public class Item : ICloneable
    {
        public Item()
        {
            this.IsEquipped = false;
            this.IsDroppable = false;
            this.Stats = new Dictionary<string, int>();
            this.EquippedStatModifiers = new Dictionary<string, int>();
            this.InventoryStatModifiers = new Dictionary<string, int>();
            this.TextFields = new List<string>();
        }

        /// <summary>
        /// The item's name. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The original name of the item pulled from raw Unit data.
        /// </summary>
        [JsonIgnore]
        public string OriginalName { get; set; }

        /// <summary>
        /// The sprite image URL for the item.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// Flag indicating if this is the unit's currently equipped item.
        /// </summary>
        public bool IsEquipped { get; set; }

        /// <summary>
        /// Flag indicating if this item will be dropped upon unit defeat.
        /// </summary>
        public bool IsDroppable { get; set; }

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
        /// The number of uses the item currently has remaining.
        /// </summary>
        public int Uses { get; set; }

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
        /// Returns a deep clone of the calling <c>Item</c> object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Item()
            {
                Name = this.Name,
                OriginalName = this.OriginalName,
                SpriteURL = this.SpriteURL,
                IsEquipped = this.IsEquipped,
                IsDroppable = this.IsDroppable,
                Category = this.Category,
                WeaponRank = this.WeaponRank,
                UtilizedStat = this.UtilizedStat,
                Uses = this.Uses,
                MaxUses = this.MaxUses,
                Stats = this.Stats.ToDictionary(entry => entry.Key, entry => entry.Value),
                EquippedStatModifiers = this.EquippedStatModifiers.ToDictionary(entry => entry.Key, entry => entry.Value),
                InventoryStatModifiers = this.InventoryStatModifiers.ToDictionary(entry => entry.Key, entry => entry.Value),
                Range = new ItemRange(this.Range.Minimum, this.Range.Maximum),
                TextFields = this.TextFields.ToList()
            };
        }
    }
}