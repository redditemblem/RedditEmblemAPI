namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedItemRangeShapeException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>ItemRangeShape</c>.
        /// </summary>
        /// <param name="itemRangeShape"></param>
        public UnmatchedItemRangeShapeException(string itemRangeShape)
            : base("item range shape", itemRangeShape)
        { }
    }
}
