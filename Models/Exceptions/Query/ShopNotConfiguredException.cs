using System;

namespace RedditEmblemAPI.Models.Exceptions.Query
{
    public class ShopNotConfiguredException : Exception
    {
        /// <summary>
        /// Thrown when the shop configuration in <c>JSONConfiguration</c> is null.
        /// </summary>
        public ShopNotConfiguredException()
            : base("Shop functionality has not been setup for this team.")
        { }
    }
}
