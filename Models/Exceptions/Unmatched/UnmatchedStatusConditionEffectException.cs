namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedStatusConditionEffectException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of a <c>StatusConditionEffect</c> type.
        /// </summary>
        /// <param name="effectName"></param>
        public UnmatchedStatusConditionEffectException(string effectName)
            : base("status condition effect", effectName)
        { }
    }
}
