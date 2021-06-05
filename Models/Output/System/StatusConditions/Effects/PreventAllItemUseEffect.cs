using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    public class PreventAllItemUseEffect : StatusConditionEffect
    {
        #region Attributes

        protected override string Name { get { return "PreventAllItemUse"; } }
        protected override int ParameterCount { get { return 0; } }

        #endregion

        public PreventAllItemUseEffect(IList<string> parameters)
            : base(parameters)
        { }

        /// <summary>
        /// Sets <c>IsUsePrevented</c> to true for every item in <paramref name="unit"/>'s inventory.
        /// </summary>
        public override void Apply(Unit unit, StatusCondition status)
        {
            //Mark use as prevented for all items in unit's inventory
            foreach (UnitInventoryItem item in unit.Inventory.Where(i => i != null))
                item.IsUsePrevented = true;
        }
    }
}
