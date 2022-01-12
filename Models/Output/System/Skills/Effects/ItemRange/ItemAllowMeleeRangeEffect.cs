using RedditEmblemAPI.Models.Output.Map;
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
        private IList<string> Categories { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public ItemAllowMeleeRangeEffect(IList<string> parameters)
            : base(parameters)
        {
            this.Categories = ParseHelper.StringCSVParse(parameters, 0);
        }

        /// <summary>
        /// Finds all items in <paramref name="unit"/>'s inventory with a category in <c>Categories</c> and sets their <c>AllowMeleeRange</c> value to true.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, IList<Unit> units)
        {
            foreach (UnitInventoryItem item in unit.Inventory)
            {
                if (item == null)
                    continue;

                //The item must have a listed category
                if (!this.Categories.Contains(item.Item.Category))
                    continue;

                //Items with a minimum range of 0 are not affected
                if (item.Item.Range.Minimum == 0)
                    continue;

                item.AllowMeleeRange = true;
            }
        }
    }
}
