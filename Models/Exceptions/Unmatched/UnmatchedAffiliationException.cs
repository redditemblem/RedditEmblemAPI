namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedAffiliationException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>Affiliation</c>.
        /// </summary>
        /// <param name="affiliation"></param>
        public UnmatchedAffiliationException(string affiliation)
            : base("affiliation", affiliation)
        { }
    }
}
