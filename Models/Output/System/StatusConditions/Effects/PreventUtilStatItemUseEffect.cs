using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    public class PreventUtilStatItemUseEffect : StatusConditionEffect
    {
        #region Attributes

        protected override string Name { get { return "PreventUtilStatItemUse"; } }
        protected override int ParameterCount { get { return 1; } }

        /// <summary>
        /// Param1. List of item utilized stats to check for.
        /// </summary>
        private IList<string> UtilizedStats { get; set; }

        #endregion

        public PreventUtilStatItemUseEffect(IList<string> parameters)
            : base(parameters)
        {
            this.UtilizedStats = DataParser.List_StringCSV(parameters, 0);

            if (this.UtilizedStats.Count == 0)
                throw new RequiredValueNotProvidedException("Param1");
        }

        public override void Apply(Unit unit, StatusCondition status)
        {
            //Mark use as prevented for all items with a utilized stat configured in UtilizedStats
            foreach(UnitInventoryItem item in unit.Inventory.Where(i => i != null && i.Item.UtilizedStats.Intersect(this.UtilizedStats).Any()))
                item.IsUsePrevented = true;
        }
    }
}
