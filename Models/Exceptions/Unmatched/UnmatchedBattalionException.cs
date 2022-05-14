namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedBattalionException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of a <c>Battalion</c>.
        /// </summary>
        public UnmatchedBattalionException(string battalionName)
            : base("battalion", battalionName)
        { }
    }
}
