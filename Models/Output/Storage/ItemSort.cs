namespace RedditEmblemAPI.Models.Output.Storage
{
    /// <summary>
    /// A sort picker option for the Convoy and Shop pages.
    /// </summary>
    public class ItemSort
    {
        /// <summary>
        /// The text that will appear inside the sort picker.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The name of the object attribute to sort on.
        /// </summary>
        public string SortAttribute { get; set; }

        /// <summary>
        /// Flag indicating if the <c>SortAttribute</c> exists on the <c>ConvoyItem</c> or <c>ShopItem</c>'s child <c>Item</c>.
        /// </summary>
        public bool IsDeepSort { get; set; }

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
