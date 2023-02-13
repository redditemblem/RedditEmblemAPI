namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedEmblemException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>Emblem</c>.
        /// </summary>
        public UnmatchedEmblemException(string emblemName)
            : base("emblem", emblemName)
        { }
    }
}
