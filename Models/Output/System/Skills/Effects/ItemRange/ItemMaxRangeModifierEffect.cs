using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange
{
    public class ItemMaxRangeModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "ItemMaxRangeModifier"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The list of <c>Item</c> categories to affect.
        /// </summary>
        private List<string> Categories { get; set; }

        /// <summary>
        /// Param2. The value by which to modifiy the <c>UnitInventoryItem</c>'s max range.
        /// </summary>
        private int Value { get; set; }

        /// <summary>
        /// Optional Param3. The filter by which to further restrict which items are affected based on their Deals Damage? value.
        /// </summary>
        private DealsDamageFilterType DealsDamageFilter { get; set; } 

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public ItemMaxRangeModifierEffect(List<string> parameters)
            : base(parameters)
        {
            this.Categories = DataParser.List_StringCSV(parameters, 0);
            this.Value = DataParser.Int_NonZeroPositive(parameters, 1, "Param2");
            this.DealsDamageFilter = GetDealsDamageFilterType(DataParser.OptionalString(parameters, 2, "Param3"));
        }

        /// <summary>
        /// Finds all items in <paramref name="unit"/>'s inventory with a category in <c>Categories</c> and boosts their max range by <c>Value</c>.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            foreach (UnitInventoryItem item in unit.Inventory.Items)
            {
                //The item must have a listed category
                if (!this.Categories.Contains(item.Item.Category))
                    continue;

                //Items with a max range of 99 are not affected
                if (item.Item.Range.Maximum == 99)
                    continue;

                //Filter based on damage dealt, if needed
                if (this.DealsDamageFilter == DealsDamageFilterType.Attack && !item.Item.DealsDamage)
                    continue;
                else if (this.DealsDamageFilter == DealsDamageFilterType.Utility && item.Item.DealsDamage)
                    continue;

                //If this modifier is greater than the one we're currently using, apply it
                if (this.Value > item.MaxRangeModifier)
                    item.MaxRangeModifier = this.Value;
            }
        }

        /// <exception cref="UnmatchedDealsDamageFilterTypeException"></exception>
        private DealsDamageFilterType GetDealsDamageFilterType(string dealsDamageFilterType)
        {
            if (string.IsNullOrEmpty(dealsDamageFilterType))
                return DealsDamageFilterType.All;

            object filterEnum;
            if (!Enum.TryParse(typeof(DealsDamageFilterType), dealsDamageFilterType, out filterEnum))
                throw new UnmatchedDealsDamageFilterTypeException(dealsDamageFilterType);

            return (DealsDamageFilterType)filterEnum;
        }

        private enum DealsDamageFilterType
        {
            All,
            Attack,
            Utility
        }
    }
}
