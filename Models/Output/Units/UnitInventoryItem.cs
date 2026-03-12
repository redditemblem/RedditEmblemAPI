using Newtonsoft.Json;
using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="UnitInventoryItem"/>
    public interface IUnitInventoryItem
    {
        /// <inheritdoc cref="UnitInventoryItem.FullName"/>
        string FullName { get; }

        /// <inheritdoc cref="UnitInventoryItem.Item"/>
        IItem Item { get; }

        /// <inheritdoc cref="UnitInventoryItem.Uses"/>
        int Uses { get; }

        /// <inheritdoc cref="UnitInventoryItem.MaxUses"/>
        int MaxUses { get; set; }

        /// <inheritdoc cref="UnitInventoryItem.Stats"/>
        Dictionary<string, IUnitInventoryItemStat> Stats { get; }

        /// <inheritdoc cref="UnitInventoryItem.MinRange"/>
        IUnitInventoryItemStat MinRange { get; }

        /// <inheritdoc cref="UnitInventoryItem.MaxRange"/>
        IUnitInventoryItemStat MaxRange { get; }

        #region Flags

        /// <inheritdoc cref="UnitInventoryItem.CanEquip"/>
        bool CanEquip { get; set; }

        /// <inheritdoc cref="UnitInventoryItem.IsPrimaryEquipped"/>
        bool IsPrimaryEquipped { get; set; }

        /// <inheritdoc cref="UnitInventoryItem.IsSecondaryEquipped"/>
        bool IsSecondaryEquipped { get; set; }

        /// <inheritdoc cref="UnitInventoryItem.IsDroppable"/>
        bool IsDroppable { get; set; }

        /// <inheritdoc cref="UnitInventoryItem.IsUsePrevented"/>
        bool IsUsePrevented { get; set; }

        /// <inheritdoc cref="UnitInventoryItem.IsNotInInventory"/>
        bool IsNotInInventory { get; set; }

        /// <inheritdoc cref="UnitInventoryItem.MaxRangeExceedsCalculationLimit"/>
        bool MaxRangeExceedsCalculationLimit { get; set; }

        /// <inheritdoc cref="UnitInventoryItem.AllowMeleeRange"/>
        bool AllowMeleeRange { get; set; }

        #endregion Flags

        /// <inheritdoc cref="UnitInventoryItem.Tags"/>
        List<ITag> Tags { get; }

        /// <inheritdoc cref="UnitInventoryItem.Engravings"/>
        List<IEngraving> Engravings { get; }

        /// <inheritdoc cref="UnitInventoryItem.HasRangeThatRequiresCalculation"/>
        bool HasRangeThatRequiresCalculation { get; }

        /// <inheritdoc cref="UnitInventoryItem.CalculateItemRanges(IUnit)"/>
        void CalculateItemRanges(IUnit unit);

        /// <inheritdoc cref="UnitInventoryItem.MatchStatName(string)"/>
        IUnitInventoryItemStat MatchStatName(string name);
    }

    #endregion Interface

    /// <summary>
    /// Object representing an <c>Item</c> present in a <c>Unit</c>'s inventory slots.
    /// </summary>
    public class UnitInventoryItem : IUnitInventoryItem
    {
        #region Attributes

        /// <summary>
        /// The full name of the item pulled from raw <c>Unit</c> data.
        /// </summary>
        [JsonIgnore]
        public string FullName { get; private set; }

        /// <summary>
        /// The <c>IItem</c> object.
        /// </summary>
        [JsonProperty("name")]
        [JsonConverter(typeof(MatchableNameConverter))]
        public IItem Item { get; private set; }

        /// <summary>
        /// The number of uses the item currently has remaining.
        /// </summary>
        public int Uses { get; private set; }

        /// <summary>
        /// The maximum number of uses the item has. For items with single or infinite uses, this value is 0. (Copied from <c>this.Item</c> on initialization & match)
        /// </summary>
        public int MaxUses { get; set; }

        /// <summary>
        /// Dictionary of the item's stats.
        /// </summary>
        public Dictionary<string, IUnitInventoryItemStat> Stats { get; private set; }

        /// <summary>
        /// The item's minimum range data.
        /// </summary>
        public IUnitInventoryItemStat MinRange { get; private set; }

        /// <summary>
        /// Thie item's maximum range data.
        /// </summary>
        public IUnitInventoryItemStat MaxRange { get; private set; }

        #region Flags

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
        /// Flag indicating whether or not this item's max range value exceeds the maximum allowed.
        /// </summary>
        public bool MaxRangeExceedsCalculationLimit { get; set; }

        /// <summary>
        /// Flag indicating that the item can be used a 1 range even when its min and max range normally don't allow it.
        /// </summary>
        public bool AllowMeleeRange { get; set; }

        #endregion Flags

        /// <summary>
        /// List of the item's tags.
        /// </summary>
        [JsonProperty(ItemConverterType = typeof(MatchableNameConverter))]
        public List<ITag> Tags { get; private set; }

        /// <summary>
        /// List of the item's engravings.
        /// </summary>
        [JsonProperty(ItemConverterType = typeof(MatchableNameConverter))]
        public List<IEngraving> Engravings { get; private set; }

        /// <summary>
        /// The engraving that overrides the item's default range values, if one exists.
        /// </summary>
        [JsonIgnore]
        private IEngraving EngravingOverridesRanges { get; set; }

        /// <summary>
        /// Is true if this item possesses a minimum or maximum range that requires calculation.
        /// </summary>
        [JsonIgnore]
        public bool HasRangeThatRequiresCalculation
        {
            get {
                return this.Item.Range.MinimumRequiresCalculation
                    || this.Item.Range.MaximumRequiresCalculation
                    || (this.EngravingOverridesRanges != null && (this.EngravingOverridesRanges.ItemRangeOverrides.MinimumRequiresCalculation || this.EngravingOverridesRanges.ItemRangeOverrides.MaximumRequiresCalculation));
            }
        }

        #endregion Attributes

        #region Constants

        private static Regex usesRegex = new Regex(@"\([0-9]+\)"); //match item uses (ex. "(5)")
        private static Regex dropRegex = new Regex(@"\(D\)");      //match item droppable (ex. "(D)")

        #endregion Constants

        /// <summary>
        /// Searches for an <c>Item</c> in <paramref name="items"/> that matches <paramref name="itemFullName"/>.
        /// </summary>
        /// <exception cref="UnmatchedItemException"></exception>
        /// <exception cref="UnmatchedEngravingException"></exception>
        public UnitInventoryItem(string itemFullName, int itemUses, IEnumerable<string> itemEngravings, IDictionary<string, IItem> items, IDictionary<string, IEngraving> engravings)
        {
            this.FullName = itemFullName;
            this.CanEquip = false;
            this.IsPrimaryEquipped = false;
            this.IsSecondaryEquipped = false;
            this.IsDroppable = false;
            this.IsUsePrevented = false;
            this.IsNotInInventory = false;
            this.Uses = 0;
            this.Stats = new Dictionary<string, IUnitInventoryItemStat>();
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
            this.Item = System.Item.MatchName(items, name);

            //Copy data over from the matched item
            this.MaxUses = this.Item.MaxUses;
            this.MinRange = new UnitInventoryItemStat(this.Item.Range.Minimum);
            this.MaxRange = new UnitInventoryItemStat(this.Item.Range.Maximum);
            this.Tags = this.Item.Tags.ToList();

            foreach (KeyValuePair<string, INamedStatValue> stat in this.Item.Stats)
                this.Stats.Add(stat.Key, new UnitInventoryItemStat(stat.Value));

            MatchEngravings(itemEngravings, engravings);   
        }

        private void MatchEngravings(IEnumerable<string> itemEngravings, IDictionary<string, IEngraving> engravings)
        {
            this.Engravings = Engraving.MatchNames(engravings, itemEngravings);
            this.Engravings = this.Engravings.Union(this.Item.Engravings).DistinctBy(e => e.Name).ToList();
            this.EngravingOverridesRanges = null;

            foreach (IEngraving engraving in this.Engravings)
            {
                //Apply any modifiers to the item's stats
                foreach (KeyValuePair<string, int> mod in engraving.ItemStatModifiers)
                {
                    IUnitInventoryItemStat stat = MatchStatName(mod.Key);
                    stat.Modifiers.TryAdd(engraving.Name, mod.Value);
                }

                if( this.EngravingOverridesRanges == null
                 && (engraving.ItemRangeOverrides.Minimum != 0 || engraving.ItemRangeOverrides.MinimumRequiresCalculation)
                 && (engraving.ItemRangeOverrides.Maximum != 0 || engraving.ItemRangeOverrides.MaximumRequiresCalculation))
                {
                    this.EngravingOverridesRanges = engraving;

                    if (engraving.ItemRangeOverrides.Minimum != 0)
                        this.MinRange.BaseValue = engraving.ItemRangeOverrides.Minimum;
                    if (engraving.ItemRangeOverrides.Maximum != 0)
                        this.MaxRange.BaseValue = engraving.ItemRangeOverrides.Maximum;
                }

                this.Tags = this.Tags.Union(engraving.Tags).ToList();
            }
        }

        /// <summary>
        /// Checks to see if either the item's minimum or maximum ranges needs to be calculated. If yes, executes on the formula.
        /// </summary>
        /// <exception cref="MinimumGreaterThanMaximumException"></exception>
        public void CalculateItemRanges(IUnit unit)
        {
            string minRangeLabel = "Minimum Range";
            string maxRangeLabel = "Maximum Range";

            IItemRange range;
            if (this.EngravingOverridesRanges != null) range = this.EngravingOverridesRanges.ItemRangeOverrides;
            else range = this.Item.Range;

            if (range.MinimumRequiresCalculation)
            {
                this.MinRange.BaseValue = CalculateItemRange(range.MinimumRaw, unit);
                minRangeLabel = "Calculated Minimum Range";
            }

            if (range.MaximumRequiresCalculation)
            {
                this.MaxRange.BaseValue = CalculateItemRange(range.MaximumRaw, unit);
                maxRangeLabel = "Calculated Maximum Range";
            }

            //After calculating, make sure our values form a valid range.
            if(this.MinRange.BaseValue == 0 && this.MaxRange.BaseValue > 0)
                throw new ItemRangeMinimumNotSetException(minRangeLabel, maxRangeLabel);
            if (this.MinRange.BaseValue > this.MaxRange.BaseValue)
                throw new MinimumGreaterThanMaximumException(minRangeLabel, maxRangeLabel);
        }

        private int CalculateItemRange(string equation, IUnit unit)
        {
            EquationParserOptions options = new EquationParserOptions()
            {
                EvalUnitStat = true
            };

            decimal equationResult = EquationParser.Evaluate(equation, unit, options);
            return Math.Max(1, Convert.ToInt32(Math.Floor(equationResult)));
        }

        public IUnitInventoryItemStat MatchStatName(string name)
        {
            IUnitInventoryItemStat stat;
            if (!this.Stats.TryGetValue(name, out stat))
                throw new UnmatchedStatException(name);

            return stat;
        }
    }
}
