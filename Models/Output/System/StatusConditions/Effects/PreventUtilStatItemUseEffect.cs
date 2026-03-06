using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
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
        private List<string> UtilizedStats { get; set; }

        #endregion

        public PreventUtilStatItemUseEffect(List<string> parameters)
            : base(parameters)
        {
            this.UtilizedStats = DataParser.List_StringCSV(parameters, INDEX_PARAM_1);

            if (!this.UtilizedStats.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_1);
        }

        public override void Apply(IUnit unit, IUnitStatus status, IDictionary<string, ITag> tags)
        {
            //Mark use as prevented for all items with a utilized stat configured in UtilizedStats
            foreach (IUnitInventoryItem item in unit.Inventory.GetAllItems().Where(i => i.Item.UtilizedStats.Intersect(this.UtilizedStats).Any()))
                item.IsUsePrevented = true;
        }
    }
}
