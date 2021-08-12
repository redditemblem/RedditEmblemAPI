using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class WeaponRankBonusProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing an <c>WeaponRankBonus</c>.
        /// </summary>
        public WeaponRankBonusProcessingException(string category, string rank, Exception innerException)
            : base("weapon rank bonus", $"{category} {rank}".Trim(), innerException)
        { }
    }
}
