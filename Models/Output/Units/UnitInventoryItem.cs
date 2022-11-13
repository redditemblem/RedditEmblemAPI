using Newtonsoft.Json;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Services.Helpers;
using System;
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
        /// The number of uses the item currently has remaining.
        /// </summary>
        public int Uses { get; set; }

        /// <summary>
        /// The calculated minimum range for the item, including modifier values.
        /// </summary>
        [JsonIgnore]
        public int ModifiedMinRangeValue { get { return this.Item.Range.Minimum + this.CalculatedMinRange + this.MinRangeModifier; } }

        /// <summary>
        /// The item's minimum range, if calculated using the unit's stats.
        /// </summary>
        public int CalculatedMinRange { get; set; }

        /// <summary>
        /// The amount by which to alter the item's minimum range.
        /// </summary>
        public int MinRangeModifier { get; set; }

        /// <summary>
        /// The calculated maximum range for the item, including modifier values.
        /// </summary>
        [JsonIgnore]
        public int ModifiedMaxRangeValue { get { return this.Item.Range.Maximum + this.CalculatedMaxRange + this.MaxRangeModifier; } }

        /// <summary>
        /// The item's maxmimum range, if calculated using the unit's stats.
        /// </summary>
        public int CalculatedMaxRange { get; set; }

        /// <summary>
        /// The amount by which to alter the item's maximum range.
        /// </summary>
        public int MaxRangeModifier { get; set; }

        /// <summary>
        /// Flag indicating whether or not this item's max range value exceeds the maximum allowed.
        /// </summary>
        public bool MaxRangeExceedsCalculationLimit { get; set; }

        /// <summary>
        /// Flag indicating that the item can be used a 1 range even when its min and max range normally don't allow it.
        /// </summary>
        public bool AllowMeleeRange { get; set; }

        #endregion

        #region Constants

        private static Regex usesRegex = new Regex(@"\([0-9]+\)"); //match item uses (ex. "(5)")
        private static Regex dropRegex = new Regex(@"\(D\)");      //match item droppable (ex. "(D)")

        #endregion Constants

        /// <summary>
        /// Searches for an <c>Item</c> in <paramref name="items"/> that matches <paramref name="fullItemName"/>.
        /// </summary>
        /// <exception cref="UnmatchedItemException"></exception>
        public UnitInventoryItem(string fullItemName, int uses, IDictionary<string, Item> items)
        {
            this.FullName = fullItemName;
            this.CanEquip = false;
            this.IsPrimaryEquipped = false;
            this.IsSecondaryEquipped = false;
            this.IsDroppable = false;
            this.IsUsePrevented = false;
            this.Uses = 0;
            this.CalculatedMinRange = 0;
            this.MinRangeModifier = 0;
            this.CalculatedMaxRange = 0;
            this.MaxRangeModifier = 0;
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
            if (uses > 0) this.Uses = uses;
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

            Item match;
            if (!items.TryGetValue(name, out match))
                throw new UnmatchedItemException(name);
            this.Item = match;
            match.Matched = true;
        }

        /// <summary>
        /// Checks to see if either the item's minimum or maximum ranges needs to be calculated. If yes, executes on the formula.
        /// </summary>
        public void CalculateItemRanges(Unit unit)
        {
            if (this.Item.Range.MinimumRequiresCalculation)
                this.CalculatedMinRange = CalculateItemRange(this.Item.Range.MinimumRaw, unit);

            if (this.Item.Range.MaximumRequiresCalculation)
                this.CalculatedMaxRange = CalculateItemRange(this.Item.Range.MaximumRaw, unit);
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
    }
}
