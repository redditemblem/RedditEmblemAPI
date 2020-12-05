namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedTagException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>Tag</c>.
        /// </summary>
        /// <param name="tagName"></param>
        public UnmatchedTagException(string tagName)
            : base("tag", tagName)
        { }
    }
}
