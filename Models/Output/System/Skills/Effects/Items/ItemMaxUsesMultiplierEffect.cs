using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange
{
    #region Interface

    /// <inheritdoc cref="ItemMaxUsesMultiplierEffect"/>
    public interface IItemMaxUsesMultiplierEffect
    {
        /// <inheritdoc cref="ItemMaxUsesMultiplierEffect.Categories"/>
        IEnumerable<string> Categories { get; }

        /// <inheritdoc cref="ItemMaxUsesMultiplierEffect.Multiplier"/>
        decimal Multiplier { get; }
    }

    #endregion Interface

    public class ItemMaxUsesMultiplierEffect : SkillEffect, IItemMaxUsesMultiplierEffect
    {
        #region Attributes

        protected override string Name { get { return "ItemMaxUsesMultiplier"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The list of <c>Item</c> categories to affect.
        /// </summary>
        public IEnumerable<string> Categories { get; private set; }

        /// <summary>
        /// Param2. The value by which to multiply the <c>UnitInventoryItem</c>'s max uses.
        /// </summary>
        public decimal Multiplier { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        public ItemMaxUsesMultiplierEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.Categories = DataParser.List_StringCSV(parameters, INDEX_PARAM_1);
            this.Multiplier = DataParser.Decimal_OneOrGreater(parameters, INDEX_PARAM_2, NAME_PARAM_2);

            if (!this.Categories.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_1);
        }

        /// <summary>
        /// Finds all items in <paramref name="unit"/>'s inventory with a category in <c>Categories</c> and multiplies their max uses by <c>Multiplier</c>.
        /// </summary>
        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            foreach (IUnitInventoryItem item in unit.Inventory.GetAllItems())
            {
                //The item must have a listed category
                if (!this.Categories.Contains(item.Item.Category))
                    continue;

                //Items with infinite max uses (i.e. 0) are not affected
                if (item.MaxUses < 1)
                    continue;

                //Calculate and set max uses
                int maxUses = (int)Math.Floor(item.MaxUses * this.Multiplier);
                if (item.MaxUses < maxUses)
                    item.MaxUses = maxUses;
            }
        }
    }
}
