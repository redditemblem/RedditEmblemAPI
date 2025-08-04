using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    public class PreventAllItemUseEffect : StatusConditionEffect
    {
        #region Attributes

        protected override string Name { get { return "PreventAllItemUse"; } }
        protected override int ParameterCount { get { return 0; } }

        #endregion

        public PreventAllItemUseEffect(List<string> parameters)
            : base(parameters)
        { }

        /// <summary>
        /// Sets <c>IsUsePrevented</c> to true for every item in <paramref name="unit"/>'s inventory.
        /// </summary>
        public override void Apply(Unit unit, UnitStatus status, IDictionary<string, Tag> tags)
        {
            //Mark use as prevented for all items in unit's inventory
            foreach (UnitInventoryItem item in unit.Inventory.GetAllItems())
                item.IsUsePrevented = true;
        }
    }
}
