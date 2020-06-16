using System;
namespace RedditEmblemAPI.Models.Exceptions.Query
{
    public class ConvoyNotConfiguredException : Exception
    {
        /// <summary>
        /// Thrown when the convoy configuration in <c>JSONConfiguration</c> is null.
        /// </summary>
        public ConvoyNotConfiguredException()
            : base("Convoy functionality has not been setup for this team.")
        { }
    }
}
