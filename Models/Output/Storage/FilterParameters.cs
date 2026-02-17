using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Storage
{
    #region Interface

    /// <inheritdoc cref="FilterParameters"/>
    public interface IFilterParameters
    {
        /// <inheritdoc cref="FilterParameters.Sorts"/>
        List<IItemSort> Sorts { get; }

        /// <inheritdoc cref="FilterParameters.Owners"/>
        IEnumerable<string> Owners { get; }

        /// <inheritdoc cref="FilterParameters.ItemCategories"/>
        IEnumerable<string> ItemCategories { get; }

        /// <inheritdoc cref="FilterParameters.UtilizedStats"/>
        IEnumerable<string> UtilizedStats { get; }

        /// <inheritdoc cref="FilterParameters.TargetedStats"/>
        IEnumerable<string> TargetedStats { get; }

        /// <inheritdoc cref="FilterParameters.FilterConditions"/>
        IDictionary<string, bool> FilterConditions { get; }
    }

    #endregion Interface

    public readonly struct FilterParameters : IFilterParameters
    {
        /// <summary>
        /// List of the options available in the sort picker.
        /// </summary>
        public List<IItemSort> Sorts { get; }

        /// <summary>
        /// List of possible owners to filter by.
        /// </summary>
        public IEnumerable<string> Owners { get; }

        /// <summary>
        /// List of all the item category values to be available as checkboxes.
        /// </summary>
        public IEnumerable<string> ItemCategories { get; }

        /// <summary>
        /// List of all the utilized stat values to be available as checkboxes.
        /// </summary>
        public IEnumerable<string> UtilizedStats { get; }

        /// <summary>
        /// List of all the targeted stat values to be available as checkboxes.
        /// </summary>
        public IEnumerable<string> TargetedStats { get; }

        /// <summary>
        /// Flags for displaying conditional filters.
        /// </summary>
        public IDictionary<string, bool> FilterConditions { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FilterParameters(List<IItemSort> sorts, IEnumerable<string> owners, IEnumerable<string> itemCategories, IEnumerable<string> utilizedStats, IEnumerable<string> targetedStats, IDictionary<string, bool> filterConditions)
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
