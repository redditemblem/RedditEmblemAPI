namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedStatusConditionException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>StatusCondition</c>.
        /// </summary>
        /// <param name="statusConditionName"></param>
        public UnmatchedStatusConditionException(string statusConditionName)
            : base("status condition", statusConditionName)
        { }
    }
}
