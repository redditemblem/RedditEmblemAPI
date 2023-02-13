namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedEngravingException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>Engraving</c>.
        /// </summary>
        public UnmatchedEngravingException(string engravingName)
            : base("engraving", engravingName)
        { }
    }
}
