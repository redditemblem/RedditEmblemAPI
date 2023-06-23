namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedBattleStyleException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>BattleStyle</c>.
        /// </summary>
        public UnmatchedBattleStyleException(string battleStyleName)
            : base("battle style", battleStyleName)
        { }
    }
}
