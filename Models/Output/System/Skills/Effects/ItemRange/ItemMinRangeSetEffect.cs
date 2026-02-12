using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

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
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        public ItemMinRangeSetEffect(List<string> parameters)
            : base(parameters)
        {
            //This needs to be executed last due to items w/ calculated ranges
            this.ExecutionOrder = SkillEffectExecutionOrder.AfterFinalStatCalculations;

            this.Categories = DataParser.List_StringCSV(parameters, INDEX_PARAM_1);
            this.Value = DataParser.Int_NonZeroPositive(parameters, INDEX_PARAM_2, NAME_PARAM_2);

            if (!this.Categories.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_1);
        }

        /// <summary>
        /// Finds all items in <paramref name="unit"/>'s inventory with a category in <c>Categories</c> and sets their minimum range to <c>Value</c>.
        /// </summary>
        public override void Apply(Unit unit, ISkill skill, MapObj map, List<Unit> units)
        {
            foreach (UnitInventoryItem item in unit.Inventory.GetAllItems())
            {
                //The item must have a listed category
                if (!this.Categories.Contains(item.Item.Category))
                    continue;

                //Items with a minimum range of 0 are not affected
                if (item.MinRange.BaseValue == 0)
                    continue;

                //Calculate the difference between the set value and the item's base min range 
                int modifier = this.Value - (int)decimal.Floor(item.MinRange.BaseValue);

                //If there is a difference and it's smaller than what we're already applying, use it
                if (modifier < 0 && modifier < item.MinRange.ForcedModifier)
                    item.MinRange.ForcedModifier = modifier;
            }
        }
    }
}
