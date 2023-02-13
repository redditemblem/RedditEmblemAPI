﻿using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange
{
    public class ItemAllowMeleeRangeEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "ItemAllowMeleeRange"; } }

        protected override int ParameterCount { get { return 1; } }

        /// <summary>
        /// Param1. The list of <c>Item</c> categories to affect.
        /// </summary>
        private List<string> Categories { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public ItemAllowMeleeRangeEffect(List<string> parameters)
            : base(parameters)
        {
            this.Categories = DataParser.List_StringCSV(parameters, 0);
        }

        /// <summary>
        /// Finds all items in <paramref name="unit"/>'s inventory with a category in <c>Categories</c> and sets their <c>AllowMeleeRange</c> value to true.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            foreach (UnitInventoryItem item in unit.Inventory.Items)
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
