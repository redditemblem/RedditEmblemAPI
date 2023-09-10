namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedAdjutantException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>Adjutant</c>.
        /// </summary>
        public UnmatchedAdjutantException(string adjutant)
            : base("adjutant", adjutant)
        { }
    }
}
