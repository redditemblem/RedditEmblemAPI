using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.ItemRange
{
    public class ItemMaxRangeModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "ItemMaxRangeModifier"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The list of <c>Item</c> categories to affect.
        /// </summary>
        private IList<string> Categories { get; set; }

        /// <summary>
        /// Param2. The value by which to modifiy the <c>UnitInventoryItem</c>'s max range.
        /// </summary>
        private int Value { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public ItemMaxRangeModifierEffect(IList<string> parameters)
            : base(parameters)
        {
            this.Categories = ParseHelper.StringCSVParse(parameters, 0);
            this.Value = ParseHelper.Int_NonZeroPositive(parameters, 1, "Param2");
        }

        /// <summary>
        /// Finds all items in <paramref name="unit"/>'s inventory with a category in <c>Categories</c> and boosts their max range by <c>Value</c>.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, IList<Unit> units)
        {
            foreach(UnitInventoryItem item in unit.Inventory)
            {
                if (item == null)
                    continue;

                //The item must have a listed category
                if (!this.Categories.Contains(item.Item.Category))
                    continue;

                //Items with a max range of 0 or 99 are not affected
                if (item.Item.Range.Maximum == 0 || item.Item.Range.Maximum == 99)
                    continue;

                //If this modifier is greater than the one we're currently using, apply it
                if(this.Value > item.MaxRangeModifier)
                    item.MaxRangeModifier = this.Value;
            }
        }
    }
}
