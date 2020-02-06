using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class MapProcessingException : Exception
    {
        public MapProcessingException(Exception innerException)
            : base("An error occurred while processing the map.", innerException)
        { }
    }
}
