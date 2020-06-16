namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedStatusConditionException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>StatusCondition</c>.
        /// </summary>
        /// <param name="statusName"></param>
        public UnmatchedStatusConditionException(string statusName)
            : base("status condition", statusName)
        { }
    }
}
