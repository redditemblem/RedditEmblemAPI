namespace RedditEmblemAPI.Models.Output.Storage
{
    #region Interface

    /// <inheritdoc cref="ItemSort"/>
    public interface IItemSort
    {
        /// <inheritdoc cref="ItemSort.DisplayName"/>
        string DisplayName { get; }

        /// <inheritdoc cref="ItemSort.SortAttribute"/>
        string SortAttribute { get; }

        /// <inheritdoc cref="ItemSort.IsDeepSort"/>
        bool IsDeepSort { get; }
    }

    #endregion Interface

    /// <summary>
    /// A sort picker option for the Convoy and Shop pages.
    /// </summary>
    public readonly struct ItemSort : IItemSort
    {
        /// <summary>
        /// The text that will appear inside the sort picker.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// The name of the object attribute to sort on.
        /// </summary>
        public string SortAttribute { get; }

        /// <summary>
        /// Flag indicating if the <c>SortAttribute</c> exists on the <c>ConvoyItem</c> or <c>ShopItem</c>'s child <c>Item</c>.
        /// </summary>
        public bool IsDeepSort { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sortDisplayName"></param>
        /// <param name="sortAttribute"></param>
        /// <param name="isDeepSort"></param>
        public ItemSort(string sortDisplayName, string sortAttribute, bool isDeepSort)
        {
            this.DisplayName = sortDisplayName;
            this.SortAttribute = sortAttribute;
            this.IsDeepSort = isDeepSort;
        }
    }
}
