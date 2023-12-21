using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Storage
{
    public class FilterParameters
    {
        /// <summary>
        /// List of the options available in the sort picker.
        /// </summary>
        public List<ItemSort> Sorts { get; set; }

        /// <summary>
        /// List of possible owners to filter by.
        /// </summary>
        public IEnumerable<string> Owners { get; set; }

        /// <summary>
        /// List of all the item category values to be available as checkboxes.
        /// </summary>
        public IEnumerable<string> ItemCategories { get; set; }

        /// <summary>
        /// List of all the utilized stat values to be available as checkboxes.
        /// </summary>
        public IEnumerable<string> UtilizedStats { get; set; }

        /// <summary>
        /// List of all the targeted stat values to be available as checkboxes.
        /// </summary>
        public IEnumerable<string> TargetedStats { get; set; }

        /// <summary>
        /// Flags for displaying conditional filters.
        /// </summary>
        public IDictionary<string, bool> FilterConditions { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FilterParameters(List<ItemSort> sorts, IEnumerable<string> owners, IEnumerable<string> itemCategories, IEnumerable<string> utilizedStats, IEnumerable<string> targetedStats, IDictionary<string, bool> filterConditions)
        {
            this.Sorts = sorts;
            this.Owners = owners;
            this.ItemCategories = itemCategories;
            this.UtilizedStats = utilizedStats;
            this.TargetedStats = targetedStats;
            this.FilterConditions = filterConditions;
        }
    }
}
