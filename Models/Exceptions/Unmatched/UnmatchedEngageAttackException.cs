namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedEngageAttackException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>EngageAttack</c>.
        /// </summary>
        public UnmatchedEngageAttackException(string engageAttackName)
            : base("engage attack", engageAttackName)
        { }
    }
}
