using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange
{
    public class ItemMinRangeSetEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "ItemMinRangeSet"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The list of <c>Item</c> categories to affect.
        /// </summary>
        private List<string> Categories { get; set; }

        /// <summary>
        /// Param2. The value by which to modifiy the <c>UnitInventoryItem</c>'s min range.
        /// </summary>
        private int Value { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public ItemMinRangeSetEffect(List<string> parameters)
            : base(parameters)
        {
            //This needs to be executed last due to items w/ calculated ranges
            this.ExecutionOrder = SkillEffectExecutionOrder.AfterFinalStatCalculations;

            this.Categories = DataParser.List_StringCSV(parameters, 0);
            this.Value = DataParser.Int_NonZeroPositive(parameters, 1, "Param2");
        }

        /// <summary>
        /// Finds all items in <paramref name="unit"/>'s inventory with a category in <c>Categories</c> and sets their minimum range to <c>Value</c>.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            foreach (UnitInventoryItem item in unit.Inventory.Items)
            {
                //The item must have a listed category
                if (!this.Categories.Contains(item.Item.Category))
                    continue;

                //Items with a minimum range of 0 are not affected
                if (item.MinRange.BaseValue == 0)
                    continue;

                //Calculate the difference between the set value and the item's base min range 
                int modifier = this.Value - item.MinRange.BaseValue;

                //If there is a difference and it's smaller than what we're already applying, use it
                if (modifier < 0 && modifier < item.MinRange.ForcedModifier)
                    item.MinRange.ForcedModifier = modifier;
            }
        }
    }
}
