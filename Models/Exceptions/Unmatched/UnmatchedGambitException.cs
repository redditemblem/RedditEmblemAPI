namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedGambitException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of a <c>Gambit</c>.
        /// </summary>
        public UnmatchedGambitException(string gambitName)
            : base("gambit", gambitName)
        { }
    }
}
