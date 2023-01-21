namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedDealsDamageFilterTypeException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>DealsDamageFilterType</c>.
        /// </summary>
        /// <param name="dealsDamageFilterType"></param>
        public UnmatchedDealsDamageFilterTypeException(string dealsDamageFilterType)
            : base("Deals Damage? filter", dealsDamageFilterType)
        { }
    }
}
