using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange
{
    public class ItemMaxUsesMultiplierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "ItemMaxUsesMultiplier"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The list of <c>Item</c> categories to affect.
        /// </summary>
        private List<string> Categories { get; set; }

        /// <summary>
        /// Param2. The value by which to multiply the <c>UnitInventoryItem</c>'s max uses.
        /// </summary>
        private decimal Multiplier { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        public ItemMaxUsesMultiplierEffect(List<string> parameters)
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
        public override void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            foreach (UnitInventoryItem item in unit.Inventory.Items)
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
