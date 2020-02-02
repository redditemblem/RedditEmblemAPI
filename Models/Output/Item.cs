using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    public class Item : ICloneable
    {
        public Item()
        {
            this.IsEquipped = false;
            this.IsDroppable = false;
            this.Stats = new Dictionary<string, int>();
            this.EquippedStatModifiers = new Dictionary<string, int>();
            this.InventoryStatModifiers = new Dictionary<string, int>();
            this.Range = new ItemRange();
            this.TextFields = new List<string>();
        }

        public string Name { get; set; }
        [JsonIgnore]
        public string OriginalName { get; set; }
        public string SpriteURL { get; set; }
        public bool IsEquipped { get; set; }
        public bool IsDroppable { get; set; }
        public string Category { get; set; }
        public string WeaponRank { get; set; }
        public string UtilizedStat { get; set; }
        public int Uses { get; set; }
        public int MaxUses { get; set; }
        public Dictionary<string, int> Stats { get; set; }
        [JsonIgnore]
        public Dictionary<string, int> EquippedStatModifiers { get; set; }
        [JsonIgnore]
        public Dictionary<string, int> InventoryStatModifiers { get; set; }
        public ItemRange Range { get; set; }
        public IList<string> TextFields { get; set; }

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
                Range = new ItemRange() { Minimum = this.Range.Minimum, Maximum = this.Range.Maximum },
                TextFields = this.TextFields.ToList()
            };
        }
    }
}