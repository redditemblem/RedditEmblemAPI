using Newtonsoft.Json;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output.Units
{
    /// <summary>
    /// Object representing an <c>Item</c> present in a <c>Unit</c>'s inventory slots.
    /// </summary>
    public class UnitInventoryItem
    {
        #region Attributes

        /// <summary>
        /// The full name of the item pulled from raw <c>Unit</c> data.
        /// </summary>
        [JsonIgnore]
        public string FullName { get; set; }

        /// <summary>
        /// Only for JSON serialization. The name of the item.
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.Item.Name; } }

        /// <summary>
        /// The <c>Item</c> object.
        /// </summary>
        [JsonIgnore]
        public Item Item { get; set; }

        /// <summary>
        /// Flag indicating if this item can be equipped by the unit.
        /// </summary>
        public bool CanEquip { get; set; }

        /// <summary>
        /// Flag indicating if this is the unit's currently equipped item.
        /// </summary>
        public bool IsEquipped { get; set; }

        /// <summary>
        /// Flag indicating if this item will be dropped upon unit defeat.
        /// </summary>
        public bool IsDroppable { get; set; }

        /// <summary>
        /// The number of uses the item currently has remaining.
        /// </summary>
        public int Uses { get; set; }

        /// <summary>
        /// The calculated minimum range for the item, including modifier values.
        /// </summary>
        [JsonIgnore]
        public int ModifiedMinRangeValue { get { return this.Item.Range.Minimum + this.MinRangeModifier; } }

        /// <summary>
        /// The amount by which to alter the item's minimum range.
        /// </summary>
        public int MinRangeModifier { get; set; }

        /// <summary>
        /// The calculated maximum range for the item, including modifier values.
        /// </summary>
        [JsonIgnore]
        public int ModifiedMaxRangeValue { get { return this.Item.Range.Maximum + this.MaxRangeModifier; } }

        /// <summary>
        /// The amount by which to alter the item's maximum range.
        /// </summary>
        public int MaxRangeModifier { get; set; }

        #endregion

        private static Regex usesRegex = new Regex(@"\([0-9]+\)"); //match item uses (ex. "(5)")
        private static Regex dropRegex = new Regex(@"\(D\)");      //match item droppable (ex. "(D)")

        /// <summary>
        /// Searches for an <c>Item</c> in <paramref name="items"/> that matches <paramref name="fullItemName"/>.
        /// </summary>
        /// <exception cref="UnmatchedItemException"></exception>
        public UnitInventoryItem(string fullItemName, IDictionary<string, Item> items)
        {
            this.FullName = fullItemName;
            this.IsDroppable = false;
            this.CanEquip = false;
            this.IsEquipped = false;
            this.Uses = 0;
            this.MinRangeModifier = 0;
            this.MaxRangeModifier = 0;

            string name = this.FullName;

            //Search for droppable syntax
            Match dropMatch = dropRegex.Match(name);
            if (dropMatch.Success)
            {
                this.IsDroppable = true;
                name = dropRegex.Replace(name, string.Empty);
            }

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

            name = name.Trim();

            Item match;
            if (!items.TryGetValue(name, out match))
                throw new UnmatchedItemException(name);
            this.Item = match;
            match.Matched = true;
        }
    }
}
