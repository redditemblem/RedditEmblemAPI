using System;

namespace RedditEmblemAPI.Models.Exceptions.Query
{
    public class TurnsNotConfiguredException : Exception
    {
        /// <summary>
        /// Thrown when the turns configuration in <c>JSONConfiguration</c> is null.
        /// </summary>
        public TurnsNotConfiguredException()
            : base("Turn submission functionality has not been setup for this team.")
        { }
    }
}
