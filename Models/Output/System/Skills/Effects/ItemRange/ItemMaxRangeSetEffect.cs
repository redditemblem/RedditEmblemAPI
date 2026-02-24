using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange
{
    #region Interface

    /// <inheritdoc cref="ItemMaxRangeSetEffect"/>
    public interface IItemMaxRangeSetEffect
    {
        /// <inheritdoc cref="ItemMaxRangeSetEffect.Categories"/>
        IEnumerable<string> Categories { get; }

        /// <inheritdoc cref="ItemMaxRangeSetEffect.Value"/>
        public int Value { get; }
    }

    #endregion Interface

    public class ItemMaxRangeSetEffect : SkillEffect, IItemMaxRangeSetEffect
    {
        #region Attributes

        protected override string Name { get { return "ItemMaxRangeSet"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The list of <c>Item</c> categories to affect.
        /// </summary>
        public IEnumerable<string> Categories { get; private set; }

        /// <summary>
        /// Param2. The value by which to modifiy the <c>UnitInventoryItem</c>'s max range.
        /// </summary>
        public int Value { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        public ItemMaxRangeSetEffect(IEnumerable<string> parameters)
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
        /// Finds all items in <paramref name="unit"/>'s inventory with a category in <c>Categories</c> and sets their max range to <c>Value</c>.
        /// </summary>
        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            foreach (IUnitInventoryItem item in unit.Inventory.GetAllItems())
            {
                //The item must have a listed category
                if (!this.Categories.Contains(item.Item.Category))
                    continue;

                //Items with a max range of 99 are not affected
                if (item.MaxRange.BaseValue >= 99)
                    continue;

                //Calculate the difference between the set value and the item's base max range 
                int modifier = this.Value - (int)decimal.Floor(item.MaxRange.BaseValue);

                //If there is a difference and it's larger than what we're already applying, use it
                if (modifier > 0 && modifier > item.MaxRange.ForcedModifier)
                    item.MaxRange.ForcedModifier = modifier;
            }
        }
    }
}
