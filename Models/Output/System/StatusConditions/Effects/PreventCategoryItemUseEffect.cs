using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    public class PreventCategoryItemUseEffect : StatusConditionEffect
    {
        #region Attributes

        protected override string Name { get { return "PreventCategoryItemUse"; } }
        protected override int ParameterCount { get { return 1; } }

        /// <summary>
        /// Param1. List of item categories to check for.
        /// </summary>
        private List<string> Categories { get; set; }

        #endregion

        public PreventCategoryItemUseEffect(List<string> parameters)
            : base(parameters)
        {
            this.Categories = DataParser.List_StringCSV(parameters, INDEX_PARAM_1);

            if (!this.Categories.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_1);
        }

        public override void Apply(Unit unit, UnitStatus status, IDictionary<string, Tag> tags)
        {
            //Mark use as prevented for all items with a category configured in Categories
            foreach (UnitInventoryItem item in unit.Inventory.GetAllItems().Where(i => this.Categories.Contains(i.Item.Category)))
                item.IsUsePrevented = true;
        }
    }
}
