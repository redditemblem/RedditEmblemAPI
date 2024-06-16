using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange
{
    public class ItemMinRangeModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "ItemMinRangeModifier"; } }
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
        public ItemMinRangeModifierEffect(List<string> parameters)
            : base(parameters)
        {
            //This needs to be executed last due to items w/ calculated ranges
            this.ExecutionOrder = SkillEffectExecutionOrder.AfterFinalStatCalculations;

            this.Categories = DataParser.List_StringCSV(parameters, INDEX_PARAM_1);
            this.Value = DataParser.Int_Negative(parameters, INDEX_PARAM_2, NAME_PARAM_2);

            if (!this.Categories.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_1);
        }

        /// <summary>
        /// Finds all items in <paramref name="unit"/>'s inventory with a category in <c>Categories</c> and lowers their min range by <c>Value</c>.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            foreach (UnitInventoryItem item in unit.Inventory.GetAllItems())
            {
                //The item must have a listed category
                if (!this.Categories.Contains(item.Item.Category))
                    continue;

                //Items with a minimum range of 0 are not affected
                if (item.MinRange.BaseValue == 0)
                    continue;

                //If this modifier is less than the one we're currently using, apply it, but don't reduce the min range below 1.
                if (this.Value < item.MinRange.ForcedModifier && this.Value + item.MinRange.BaseValue > 0)
                    item.MinRange.ForcedModifier = this.Value;
            }
        }
    }
}
