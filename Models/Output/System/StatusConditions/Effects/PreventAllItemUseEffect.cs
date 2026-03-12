using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    #region Interface

    /// <inheritdoc cref="PreventAllItemUseEffect"/>
    public interface IPreventAllItemUseEffect { }

    #endregion Interface

    public class PreventAllItemUseEffect : StatusConditionEffect, IPreventAllItemUseEffect
    {
        #region Attributes

        protected override string Name { get { return "PreventAllItemUse"; } }
        protected override int ParameterCount { get { return 0; } }

        #endregion Attributes

        public PreventAllItemUseEffect(IEnumerable<string> parameters)
            : base(parameters)
        { }

        /// <summary>
        /// Sets <c>IsUsePrevented</c> to true for every item in <paramref name="unit"/>'s inventory.
        /// </summary>
        public override void Apply(IUnit unit, IUnitStatus status, IDictionary<string, ITag> tags)
        {
            //Mark use as prevented for all items in unit's inventory
            foreach (IUnitInventoryItem item in unit.Inventory.GetAllItems())
                item.IsUsePrevented = true;
        }
    }
}
