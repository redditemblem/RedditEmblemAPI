using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    public class FilterParameters
    {
        public IList<ItemSort> Sorts { get; set; }

        public IList<string> ItemCategories { get; set; }

        public IList<string> UtilizedStats { get; set; }

        public IDictionary<string, bool> Parameters { get; set; }

        public FilterParameters(IList<ItemSort> sorts, IList<string> itemCategories, IList<string> utilizedStats, IDictionary<string, bool> parameters)
        {
            this.Sorts = sorts;
            this.ItemCategories = itemCategories;
            this.UtilizedStats = utilizedStats;
            this.Parameters = parameters;
        }
    }
}
