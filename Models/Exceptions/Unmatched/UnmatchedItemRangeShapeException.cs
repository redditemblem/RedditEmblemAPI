using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedItemRangeShapeException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>ItemRangeShape</c>.
        /// </summary>
        public UnmatchedItemRangeShapeException(string itemRangeShape, IEnumerable<string> possibleValues)
            : base("item range shape", itemRangeShape, possibleValues)
        { }
    }
}
