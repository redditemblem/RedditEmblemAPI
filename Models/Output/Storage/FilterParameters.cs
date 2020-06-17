using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Storage
{
    public class FilterParameters
    {
        /// <summary>
        /// List of the options available in the sort picker.
        /// </summary>
        public IList<ItemSort> Sorts { get; set; }

        /// <summary>
        /// List of all the item category values to be available as checkboxes.
        /// </summary>
        public IList<string> ItemCategories { get; set; }

        /// <summary>
        /// List of all the utilized stat values to be available as checkboxes.
        /// </summary>
        public IList<string> UtilizedStats { get; set; }

        /// <summary>
        /// Flags for displaying conditional filters.
        /// </summary>
        public IDictionary<string, bool> FilterConditions { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sorts"></param>
        /// <param name="itemCategories"></param>
        /// <param name="utilizedStats"></param>
        /// <param name="filterConditions"></param>
        public FilterParameters(IList<ItemSort> sorts, IList<string> itemCategories, IList<string> utilizedStats, IDictionary<string, bool> filterConditions)
        {
            this.Sorts = sorts;
            this.ItemCategories = itemCategories;
            this.UtilizedStats = utilizedStats;
            this.FilterConditions = filterConditions;
        }
    }
}
