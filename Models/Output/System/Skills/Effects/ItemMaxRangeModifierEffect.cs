﻿using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class ItemMaxRangeModifierEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. The list of <c>Item</c> categories to affect.
        /// </summary>
        public IList<string> Categories { get; set; }

        /// <summary>
        /// Param2. The value by which to modifiy the <c>UnitInventoryItem</c>'s max range.
        /// </summary>
        public int Value { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public ItemMaxRangeModifierEffect(IList<string> parameters)
        {
            if (parameters.Count < 2)
                throw new SkillEffectMissingParameterException("ItemMaxRangeModifier", 2, parameters.Count);

            this.Categories = ParseHelper.StringCSVParse(parameters.ElementAt<string>(0));
            this.Value = ParseHelper.SafeIntParse(parameters.ElementAt<string>(1), "Param2", false);
        }

        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            foreach(UnitInventoryItem item in unit.Inventory)
            {
                if (item == null)
                    continue;

                //The item must have a listed category
                if (!this.Categories.Contains(item.Item.Category))
                    continue;

                //Items with a max range of 0 are not affected
                if (item.Item.Range.Maximum == 0)
                    continue;

                int modifier = this.Value;

                //The item's max range cannot be reduced below its minimum range
                if (item.Item.Range.Maximum + modifier < item.Item.Range.Minimum)
                    modifier = item.Item.Range.Maximum - item.Item.Range.Minimum;

                item.MaxRangeModifier = modifier;
            }
        }
    }
}
