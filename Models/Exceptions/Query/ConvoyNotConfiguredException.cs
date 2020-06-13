using System;
namespace RedditEmblemAPI.Models.Exceptions.Query
{
    public class ConvoyNotConfiguredException : Exception
    {
        public ConvoyNotConfiguredException()
            : base("Convoy functionality has not been setup for this team.")
        { }
    }
}
