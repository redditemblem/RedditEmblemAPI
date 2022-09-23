namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedClassException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>Class</c>.
        /// </summary>
        /// <param name="className"></param>
        public UnmatchedClassException(string className)
            : base("class", className)
        { }
    }
}
