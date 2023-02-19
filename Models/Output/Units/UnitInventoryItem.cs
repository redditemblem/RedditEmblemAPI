using Newtonsoft.Json;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// The <c>Item</c> object.
        /// </summary>
        [JsonIgnore]
        public Item Item { get; set; }

        /// <summary>
        /// Flag indicating if this item can be equipped by the unit.
        /// </summary>
        public bool CanEquip { get; set; }

        /// <summary>
        /// Flag indicating if this is the unit's current primary equipped item. Should only ever be true for one item.
        /// </summary>
        public bool IsPrimaryEquipped { get; set; }

        /// <summary>
        /// Flag indicating if this is one of the unit's secondary equipped items. Can be true for multiple items.
        /// </summary>
        public bool IsSecondaryEquipped { get; set; }

        /// <summary>
        /// Flag indicating if this item will be dropped upon unit defeat.
        /// </summary>
        public bool IsDroppable { get; set; }

        /// <summary>
        /// Flag indicating if an item cannot currently be used by the unit.
        /// </summary>
        public bool IsUsePrevented { get; set; }

        /// <summary>
        /// Flag indicating if an equipped item was not found in the unit's inventory, but permitted anyways.
        /// </summary>
        public bool IsNotInInventory { get; set; }

        /// <summary>
        /// The number of uses the item currently has remaining.
        /// </summary>
        public int Uses { get; set; }

        /// <summary>
        /// The maximum number of uses the item has. For items with single or infinite uses, this value is 0. (Copied from <c>this.Item</c> on initialization & match)
        /// </summary>
        public int MaxUses { get; set; }

        /// <summary>
        /// Dictionary of the item's stats.
        /// </summary>
        public Dictionary<string, UnitInventoryItemStat> Stats { get; set; }

        /// <summary>
        /// The item's minimum range data.
        /// </summary>
        public UnitInventoryItemStat MinRange { get; set; }

        /// <summary>
        /// Thie item's maximum range data.
        /// </summary>
        public UnitInventoryItemStat MaxRange { get; set; }

        /// <summary>
        /// Flag indicating whether or not this item's max range value exceeds the maximum allowed.
        /// </summary>
        public bool MaxRangeExceedsCalculationLimit { get; set; }

        /// <summary>
        /// Flag indicating that the item can be used a 1 range even when its min and max range normally don't allow it.
        /// </summary>
        public bool AllowMeleeRange { get; set; }

        /// <summary>
        /// List of the item's engravings.
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
        /// For JSON Serialization ONLY. Complete list of the item's engravings.
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> Engravings { get { return this.EngravingsList.Select(e => e.Name).Union(this.Item.Engravings.Select(e => e.Name)).Distinct(); } }

        #endregion JSON Serialization Only

        #endregion

        #region Constants

        private static Regex usesRegex = new Regex(@"\([0-9]+\)"); //match item uses (ex. "(5)")
        private static Regex dropRegex = new Regex(@"\(D\)");      //match item droppable (ex. "(D)")

        #endregion Constants

        /// <summary>
        /// Searches for an <c>Item</c> in <paramref name="items"/> that matches <paramref name="itemFullName"/>.
        /// </summary>
        /// <exception cref="UnmatchedItemException"></exception>
        /// <exception cref="UnmatchedEngravingException"></exception>
        public UnitInventoryItem(string itemFullName, int itemUses, IEnumerable<string> itemEngravings, IDictionary<string, Item> items, IDictionary<string, Engraving> engravings)
        {
            this.FullName = itemFullName;
            this.CanEquip = false;
            this.IsPrimaryEquipped = false;
            this.IsSecondaryEquipped = false;
            this.IsDroppable = false;
            this.IsUsePrevented = false;
            this.IsNotInInventory = false;
            this.Uses = 0;
            this.Stats = new Dictionary<string, UnitInventoryItemStat>();
            this.AllowMeleeRange = false;

            string name = this.FullName;

            //Search for droppable syntax
            Match dropMatch = dropRegex.Match(name);
            if (dropMatch.Success)
            {
                this.IsDroppable = true;
                name = dropRegex.Replace(name, string.Empty);
            }

            //If uses is a separate field, use that value. Else, look for "(#)" syntax.
            if (itemUses > 0) this.Uses = itemUses;
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

            //Copy data over from the matched item
            this.MaxUses = this.Item.MaxUses;
            this.MinRange = new UnitInventoryItemStat(this.Item.Range.Minimum);
            this.MaxRange = new UnitInventoryItemStat(this.Item.Range.Maximum);

            foreach (KeyValuePair<string, int> stat in this.Item.Stats)
                this.Stats.Add(stat.Key, new UnitInventoryItemStat(stat.Value));

            MatchEngravings(itemEngravings, engravings);   
        }

        private void MatchEngravings(IEnumerable<string> itemEngravings, IDictionary<string, Engraving> engravings)
        {
            this.EngravingsList = Engraving.MatchNames(engravings, itemEngravings);
            foreach (Engraving engraving in this.EngravingsList.Union(this.Item.Engravings))
            {
                //Apply any modifiers to the item's stats
                foreach (KeyValuePair<string, int> mod in engraving.StatModifiers)
                {
                    UnitInventoryItemStat stat = MatchStatName(mod.Key);
                    stat.Modifiers.TryAdd(engraving.Name, mod.Value);
                }
            }
        }

        /// <summary>
        /// Checks to see if either the item's minimum or maximum ranges needs to be calculated. If yes, executes on the formula.
        /// </summary>
        public void CalculateItemRanges(Unit unit)
        {
            if (this.Item.Range.MinimumRequiresCalculation)
                this.MinRange.BaseValue = CalculateItemRange(this.Item.Range.MinimumRaw, unit);

            if (this.Item.Range.MaximumRequiresCalculation)
                this.MaxRange.BaseValue = CalculateItemRange(this.Item.Range.MaximumRaw, unit);
        }

        private int CalculateItemRange(string equation, Unit unit)
        {
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalUnitStat = true
            };

            decimal equationResult = EquationParser.Evaluate(equation, unit, options);
            return Math.Max(1, Convert.ToInt32(Math.Floor(equationResult)));
        }

        public UnitInventoryItemStat MatchStatName(string name)
        {
            UnitInventoryItemStat stat;
            if (!this.Stats.TryGetValue(name, out stat))
                throw new UnmatchedStatException(name);

            return stat;
        }
    }
}
