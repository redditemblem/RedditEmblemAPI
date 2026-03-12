using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange
{
    #region Interface

    /// <inheritdoc cref="ItemAllowMeleeRangeEffect"/>
    public interface IItemAllowMeleeRangeEffect
    {
        /// <inheritdoc cref="ItemAllowMeleeRangeEffect.Categories"/>
        IEnumerable<string> Categories { get; }
    }

    #endregion Interface

    public class ItemAllowMeleeRangeEffect : SkillEffect, IItemAllowMeleeRangeEffect
    {
        #region Attributes

        protected override string Name { get { return "ItemAllowMeleeRange"; } }

        protected override int ParameterCount { get { return 1; } }

        /// <summary>
        /// Param1. The list of <c>Item</c> categories to affect.
        /// </summary>
        public IEnumerable<string> Categories { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        public ItemAllowMeleeRangeEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.Categories = DataParser.List_StringCSV(parameters, INDEX_PARAM_1);

            if (!this.Categories.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_1);
        }

        /// <summary>
        /// Finds all items in <paramref name="unit"/>'s inventory with a category in <c>Categories</c> and sets their <c>AllowMeleeRange</c> value to true.
        /// </summary>
        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            foreach (IUnitInventoryItem item in unit.Inventory.GetAllItems())
            {
                //The item must have a listed category
                if (!this.Categories.Contains(item.Item.Category))
                    continue;

                //Items with a minimum range of 0 are not affected
                if (item.MinRange.BaseValue == 0 && !item.Item.Range.MinimumRequiresCalculation)
                    continue;

                item.AllowMeleeRange = true;
            }
        }
    }
}
