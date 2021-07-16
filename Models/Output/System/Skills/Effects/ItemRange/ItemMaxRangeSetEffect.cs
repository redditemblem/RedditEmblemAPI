using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange
{
    public class ItemMaxRangeSetEffect : SkillEffect
    {
        #region Attributes
        protected override string Name { get { return "ItemMaxRangeSet"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The list of <c>Item</c> categories to affect.
        /// </summary>
        private IList<string> Categories { get; set; }

        /// <summary>
        /// Param2. The value by which to modifiy the <c>UnitInventoryItem</c>'s max range.
        /// </summary>
        private int Value { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RangeMaximumTooLargeException"></exception>
        public ItemMaxRangeSetEffect(IList<string> parameters)
            : base(parameters)
        {
            this.Categories = ParseHelper.StringCSVParse(parameters, 0);
            this.Value = ParseHelper.Int_NonZeroPositive(parameters, 1, "Param2");

            if (this.Value > 15)
                throw new RangeMaximumTooLargeException("For performance reasons, item ranges in excess of 15 tiles are currently not allowed.");
        }

        /// <summary>
        /// Finds all items in <paramref name="unit"/>'s inventory with a category in <c>Categories</c> and sets their max range to <c>Value</c>.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, IList<Unit> units)
        {
            foreach(UnitInventoryItem item in unit.Inventory)
            {
                if (item == null)
                    continue;

                //The item must have a listed category
                if (!this.Categories.Contains(item.Item.Category))
                    continue;

                //Items with a max range of 99 are not affected
                if (item.Item.Range.Maximum == 99)
                    continue;

                //Calculate the difference between the set value and the item's base max range 
                int modifier = this.Value - item.Item.Range.Maximum;

                //If there is a difference and it's larger than what we're already applying, use it
                if (modifier > 0 && modifier > item.MaxRangeModifier)
                    item.MaxRangeModifier = modifier;
            }
        }
    }
}
