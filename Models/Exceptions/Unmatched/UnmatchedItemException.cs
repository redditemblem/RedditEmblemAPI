using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedItemException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>Item</c>.
        /// </summary>
        /// <param name="itemName"></param>
        public UnmatchedItemException(string itemName)
            : base("item", itemName)
        { }
    }
}
