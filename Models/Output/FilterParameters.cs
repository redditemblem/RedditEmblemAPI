using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    public class FilterParameters
    {
        public IList<ItemSort> Sorts { get; set; }

        public IList<string> ItemCategories { get; set; }

        public IList<string> UtilizedStats { get; set; }

        public IDictionary<string, bool> DisplayFilters { get; set; }

        public FilterParameters(IList<ItemSort> sorts, IList<string> itemCategories, IList<string> utilizedStats, IDictionary<string, bool> displayFilters)
        {
            this.Sorts = sorts;
            this.ItemCategories = itemCategories;
            this.UtilizedStats = utilizedStats;
            this.DisplayFilters = displayFilters;
        }
    }
}
